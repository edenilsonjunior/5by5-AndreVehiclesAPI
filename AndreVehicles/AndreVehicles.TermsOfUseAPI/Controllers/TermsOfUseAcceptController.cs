using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Financials;
using Models.Financials;
using Models.People;
using Services.Financials;
using Services.People;

namespace AndreVehicles.TermsOfUseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TermsOfUseAcceptController : ControllerBase
{
    private readonly TermsOfUseAcceptService _termsOfUseAcceptService;
    private readonly TermsOfUseService _termsOfUseService;
    private readonly CustomerService _customerService;

    public TermsOfUseAcceptController(TermsOfUseAcceptService termsOfUseAcceptService, TermsOfUseService termsOfUseService)
    {
        _termsOfUseAcceptService = termsOfUseAcceptService;
        _termsOfUseService = termsOfUseService;
        _customerService = new CustomerService();
    }



    [HttpGet]
    public ActionResult<List<TermsOfUseAccept>> Get() => _termsOfUseAcceptService.Get();



    [HttpGet]
    [Route("{id:length(24)}", Name = "GetTermsOfUseAccept")]
    public ActionResult<TermsOfUseAccept> Get(string id)
    {
        var termsOfUseAccept = _termsOfUseAcceptService.Get(id);

        if (termsOfUseAccept == null)
        {
            return NotFound();
        }

        return termsOfUseAccept;
    }

    [HttpPost]
    public ActionResult<TermsOfUseAccept> Post(TermsOfUseAcceptDTO termsOfUseAcceptDTO)
    {
        TermsOfUse termsOfUse = _termsOfUseService.Get(termsOfUseAcceptDTO.TermsOfUseId);

        if(termsOfUse == null)
        {
            return NotFound("Can't find Terms of Use!");
        }

        Customer customer = _customerService.Get("dapper",termsOfUseAcceptDTO.CustomerDocument);

        if (customer == null)
        {
            return NotFound("Can't find Customer!");
        }

        TermsOfUseAccept termsOfUseAccept = new()
        {
            Customer = customer,
            AcceptDate = termsOfUseAcceptDTO.AcceptDate,
            TermsOfUse = termsOfUse
        };

        _termsOfUseAcceptService.Create(termsOfUseAccept);

        return CreatedAtRoute("GetTermsOfUseAccept", new { id = termsOfUseAccept.Id.ToString() }, termsOfUseAccept);
    }
}
