﻿using AutoMapper;
using FoodYeah.Commons;
using FoodYeah.Dto;
using FoodYeah.Model;
using FoodYeah.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FoodYeah.Commons.Enums;

namespace FoodYeah.Service.Impl
{
    public class QuoteDetailServiceImpl : QuoteDetailService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly TransactionService _transactionService;

        public QuoteDetailServiceImpl(ApplicationDbContext context,
          IMapper mapper, TransactionService transactionService)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
        }
        public decimal ConvertToTea(Enums.TypeRate type, decimal rate)
        {
            if(type == (TypeRate)2)
            {
                decimal TNA = Convert.ToDecimal((Math.Pow(Decimal.ToDouble( 1+(rate / 360)), 360)) - 1);
                return TNA;
            }
            if (type == (TypeRate)3)
            {
                return rate;
            }

                return rate;
            
        }

        public decimal CurrencyConverter(decimal price)
        {

            var client = new RestClient("https://api.currencyfreaks.com/latest?apikey=da06e3bb1f1d4da08b8e5cde85590ed8&symbols=PEN,USD'");

            client.Timeout = -1;

            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            var values = JsonConvert.DeserializeObject<CurrencyDto>(response.Content);
            decimal change = Convert.ToDecimal(values.Rates.PEN);

            decimal valuechange = price / change;

            return valuechange;
        }


        public QuoteDetailsDto Create(CreateQuoteDetailsDto model,decimal totalPrice)
        {
            var price =new decimal();
            if (model.Currency == "Soles")
                 price = totalPrice;
            else
            {
                price = CurrencyConverter(totalPrice);
            }

            decimal tasa = (_context.LOCs.Single(x => x.LOCId == model.LocId).Rate / 100);
            var TypeRate = (_context.LOCs.Single(x => x.LOCId == model.LocId).TypeRate);
            decimal newTasa = ConvertToTea(TypeRate, tasa);

            double numerobase = 1 + Decimal.ToDouble(newTasa);
            decimal potencia = Convert.ToDecimal(model.Frecuency) / 360m;

            decimal tasaConvertida = Convert.ToDecimal(Math.Pow(numerobase, Decimal.ToDouble(potencia)) - 1);
            
            decimal e = Convert.ToDecimal(Math.Pow((1 + Decimal.ToDouble(tasaConvertida)), model.NumberQuotes));
            decimal quote = price * ((tasaConvertida * e)/(e - 1));

            List<Quote> cuotas = new List<Quote>();
            decimal Total = 0m;
            var primerDiaDePago = DateTime.Today;
            for (int i = 0; i < model.NumberQuotes; i++)
            {
               
                var ultimoDiaDePago = primerDiaDePago.AddDays(model.Frecuency);
                quote = Math.Round(quote, 1);
                cuotas.Add(new Quote { Value = quote, FirstPaidDay = primerDiaDePago, LastPaidDay = ultimoDiaDePago });
                primerDiaDePago = ultimoDiaDePago.AddDays(model.Frecuency);
                Total += quote;
                
            }


           
            var entry = new QuoteDetail
            {
                NumberQuotes = model.NumberQuotes,
                Frecuency = model.Frecuency,
                InterestRate = tasa,
                Currency = model.Currency,
                LocId = model.LocId,
                Quotes = cuotas,
                Debt = cuotas[0].Value,
                LastTotal = Total,
            };
            
            _context.QuoteDetails.Add(entry);
            _context.SaveChanges();

            return _mapper.Map<QuoteDetailsDto>(entry);

        }

        public void Remove(int id)
        {
            var target = _context.QuoteDetails.Single(x => x.QuoteDetailsId == id);
            _context.QuoteDetails.Remove(target);
            _context.SaveChanges();

        }
        public DataCollection<QuoteDetailsDto> GetAll(int page, int take)
        {
            return _mapper.Map<DataCollection<QuoteDetailsDto>>(
                 _context.QuoteDetails.OrderByDescending(x => x.QuoteDetailsId)
                               .Include(x => x.Loc)
                               .Include(x=>x.Quotes)
                              .AsQueryable()
                              .Paged(page, take)
            );
        }

        public QuoteDetailsDto GetById(int id)
        {
            return _mapper.Map<QuoteDetailsDto>(
                _context.QuoteDetails.Single(x => x.QuoteDetailsId == id)
           );
        }

        public void Update(int id, UpdateQuoteDetailsDto model)
        {
            throw new NotImplementedException();
        }

        public void PayQuote(decimal amount, int quoteDetailId)
        {
            var qd = _context.QuoteDetails.Single(x => x.QuoteDetailsId == quoteDetailId);
            var DeudaTotal = 0m;
            var loc = _context.LOCs.Single(x => x.LOCId == qd.LocId);
            loc.AvalibleLineOfCredit = loc.AvalibleLineOfCredit + amount;
            if(qd.Quotes.Count > 0)
            {
                if(amount == qd.Quotes.ElementAt(0).Value)
                {
                    
                    _transactionService.Create(new TransactionCreateDto
                    {
                        CustomerId = loc.CustomerId,
                        Status = "Accepted ",
                        Description = "Se ha pagado la cuota total con el valor de " + qd.Quotes.ElementAt(0).ToString()
                    });
                    qd.Quotes.RemoveAt(0);
                }
                else if(amount > qd.Quotes.ElementAt(0).Value) {
                    
                        foreach (Quote quote in qd.Quotes.ToList())
                        {
                        
                            if (amount > quote.Value)
                            {
                                _transactionService.Create(new TransactionCreateDto
                                {
                                    CustomerId = loc.CustomerId,
                                    Status = "Accepted ",
                                    Description = "Se ha pagado la cuota total con el valor de " + quote.ToString()
                                });
                                amount -= quote.Value;
                                qd.Quotes.RemoveAt(0);

                            }
                            else
                            {
                                _transactionService.Create(new TransactionCreateDto
                                {
                                    CustomerId = loc.CustomerId,
                                    Status = "Accepted ",
                                    Description = "Se ha pagado parcialmente la cuota  el valor de " + amount.ToString()
                                });
                                var cuota = qd.Quotes.ElementAt(0);
                                qd.Quotes.RemoveAt(0);
                                cuota.Value -= amount;
                                qd.Quotes.Insert(0, cuota);
                                qd.LastTotal -= amount;
                                amount = 0;
                            }
                        if (amount == 0) break;
                    }
                        
                    }
                else if(amount < qd.Quotes.ElementAt(0).Value)
                {
                    _transactionService.Create(new TransactionCreateDto
                    {
                        CustomerId = loc.CustomerId,
                        Status = "Accepted ",
                        Description = "Se ha pagado parcialmente la cuota  el valor de " + amount.ToString()
                    });

                    var cuota = qd.Quotes.ElementAt(0);
                    qd.Quotes.RemoveAt(0);
                    cuota.Value -= amount;
                    qd.Quotes.Insert(0, cuota);
                }
                if (qd.Quotes.Count == 0)
                {
                    _context.QuoteDetails.Remove(qd);
                }


                else {
                    qd.Debt = qd.Quotes.ElementAt(0).Value;
                    foreach (Quote quote in qd.Quotes.ToList())
                        DeudaTotal += quote.Value;
                    qd.LastTotal = DeudaTotal;
                }
                
                _context.SaveChanges();

            }
            else { return; }
        }

       
    }
}
