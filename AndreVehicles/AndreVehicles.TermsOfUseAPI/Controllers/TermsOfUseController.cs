using Microsoft.AspNetCore.Mvc;
using Models.Financials;
using Services.Financials;

namespace AndreVehicles.TermsOfUseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TermsOfUseController : ControllerBase
{
    private readonly TermsOfUseService _termOfUseService;

    public TermsOfUseController(TermsOfUseService termOfUseService)
    {
        _termOfUseService = termOfUseService;
    }

    [HttpGet]
    public ActionResult<List<TermsOfUse>> Get()
    {
        var termOfUse = _termOfUseService.Get();
        if (termOfUse == null)
            return NotFound();
        

        return termOfUse;
    }

    [HttpGet("{id:length(24)}", Name = "GetTermOfUse")]
    public ActionResult<TermsOfUse> Get(string id)
    {
        var termOfUse = _termOfUseService.Get(id);
        if (termOfUse == null)
            return NotFound();

        return termOfUse;
    }

    [HttpPost]
    public ActionResult<TermsOfUse> Create(TermsOfUse termOfUse)
    {
        _termOfUseService.Create(termOfUse);

        return CreatedAtRoute("GetTermOfUse", new { id = termOfUse.Id.ToString() }, termOfUse);
    }
}
