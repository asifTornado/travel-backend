using AutoMapper;
using backEnd.Helpers.IHelpers;
using backEnd.Models;
using backEnd.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backEnd.Mappings;
using System.Diagnostics;
using Org.BouncyCastle.Asn1;

namespace backEnd.Controllers;




[Route("/")]
[ApiController]

public class QuotationController : ControllerBase
{

    private IQuotationService _quotationService;

    public QuotationController(IQuotationService quotationService)
    {
        _quotationService = quotationService;

    }
  
 [HttpPost]
 [Route("editQuotation")]
 public async Task<IActionResult> EditQuotation(Quotation quotation){

    await _quotationService.UpdateQuotation(quotation);

    return Ok(true);
     
 }




}
