using backEnd.Models;
using backEnd.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using System.IO;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using Dapper;
using System.Data;
using System.Linq;
using System;


namespace backEnd.Services;



public class MoneyReceiptService
{

     

     private IConnection _connection;

     private TravelContext _context;

   public MoneyReceiptService(IConnection connection, TravelContext context)
   {
        _connection = connection;
        _context = context;
   }


  public async Task SubmitMoneyReceipt(MoneyReceipt moneyReceipt, Request request){
    _context.Entry(moneyReceipt).State = EntityState.Added;
    _context.Entry(request).State = EntityState.Modified;
    await _context.SaveChangesAsync();
  }

    public async Task UpdateMoneyReceipt(MoneyReceipt moneyReceipt){
    _context.Entry(moneyReceipt).State = EntityState.Modified;
    await _context.SaveChangesAsync();
  }


  
  public async Task<List<MoneyReceipt>> GetMoneyReceiptsForMe(User user){
    var result = await _context.MoneyReceipts.AsNoTracking()
      .Include(x => x.CurrentHandler)
                 .Where(x => x.CurrentHandlerId == user.Id)
                 .ToListAsync();
    return result;
  }


    public async Task<List<MoneyReceipt>> GetMyMoneyReceipts(User user){
    var result = await _context.MoneyReceipts.AsNoTracking()
                 .Include(x => x.Request)
                   .Include(x => x.CurrentHandler)
                 .Where(x => x.Request.RequesterId == user.Id)
                 .ToListAsync();
    return result;
  }

    public async Task<List<MoneyReceipt>> GetMoneyReceiptsProcessedByMe(User user){
    var result = await _context.MoneyReceipts.AsNoTracking()
    .Include(x => x.CurrentHandler)
    .ToListAsync();
    var finalResult = result.Where(x => x.Approvals.Any(y => y.Id == user.Id)).ToList();
    return finalResult;
  }

    public async Task<List<MoneyReceipt>> GetAllMoneyReceipts(){
    var result = await _context.MoneyReceipts.AsNoTracking()
                 .Include(x => x.CurrentHandler)
                 .ToListAsync();
    return result;
  }

  public async Task<MoneyReceipt> GetMoneyReceipt(int id){
    var result = await _context.MoneyReceipts.AsNoTracking()
                 .Include(x => x.Request)
                 .Where(x => x.Id == id)
                 .FirstOrDefaultAsync();
    return result;
  }
}