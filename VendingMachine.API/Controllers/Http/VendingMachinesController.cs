using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VendingMachine.API.Dto.VendingMachine;
using VendingMachine.Core.Exceptions;
using VendingMachine.Core.Models.Exceptions;
using VendingMachine.Core.Services;

namespace VendingMachine.API.Controllers.Http
{
    [ApiController]
    [Authorize, Route("[controller]")]
    public class VendingMachinesController(
        VendingMachinesService vendingMachinesService, 
        DrinksService drinksService) : Controller
    {
        private readonly VendingMachinesService vendingMachinesService = vendingMachinesService;
        private readonly DrinksService drinksService = drinksService;

        [Authorize(Roles = "Admin"), HttpPost]
        public async Task<ActionResult<Guid>> Add([FromBody] AddVendingMachineRequest req)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var id = await vendingMachinesService.Add(
                    req.TitleVendingMachine,
                    req.AmountMoneyVendingMachine!.Value);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Authorize(Roles = "Admin"), HttpPatch]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateVendingMachineRequest req)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var id = await vendingMachinesService.Update(
                    req.IdVendingMachine,
                    req.TitleVendingMachine!,
                    req.AmountMoneyVendingMachine!.Value);

                return Ok(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Authorize(Roles = "Admin"), HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete([FromRoute] Guid id)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, "Недостаток данных"));

                var idVendingMachine = await vendingMachinesService.Delete(id);

                return Ok(idVendingMachine);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetVendingMachineResponse?>> GetById([FromRoute] Guid id)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var vendingMachine = await vendingMachinesService.GetById(id);
                var drinks = await drinksService.GetAllByVendingId(id);

                var vendingMachineResponse = new GetVendingMachineResponse(
                    vendingMachine.Id,
                    vendingMachine.Title,
                    vendingMachine.AmountMoney,
                    drinks);

                return Ok(vendingMachineResponse);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetVendingMachineResponse>?>> GetAll()
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var vendingMachines = await vendingMachinesService.GetAll();

                return Ok(vendingMachines);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}
