using Autoplace.Administration.Common;
using Autoplace.Administration.Models.InputModels;
using Autoplace.Administration.Models.OutputModels;
using Autoplace.Administration.Services;
using Autoplace.Common.Controllers;
using Autoplace.Common.Enums;
using Autoplace.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Autoplace.Administration.Controllers
{
    public class ApprovalRequestsController : BaseAdminApiController
    {
        private readonly IApprovalRequestsService approvalRequestsService;

        public ApprovalRequestsController(IApprovalRequestsService approvalRequestsService)
        {
            this.approvalRequestsService = approvalRequestsService;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ApprovalRequestOutputModel>> Approve(int id, RequestApprovalInputModel input)
        {
            var result = await approvalRequestsService.ChangeStatusAsync(id, input);

            if (!result.IsSuccessful)
            {
                return BadRequest(ApiResponse.Failure(result.ErrorMessages));
            }

            return Ok(result.Model);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalRequestOutputModel>>> GetAllForApproval(int? pageSize, int? page)
        {
            if (pageSize != null && page != null)
            {
                var approvalRequestsWithPagination = await approvalRequestsService.GetAllAsync(ar => ar.Status == AutopartStatus.WaitingForApproval, pageSize.Value, page.Value);
                return Ok(approvalRequestsWithPagination);
            }

            var approvalRequests = await approvalRequestsService.GetAllAsync(ar => ar.Status == AutopartStatus.WaitingForApproval);
            return Ok(approvalRequests);
        }

        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<ApprovalRequestOutputModel>>> GetAllArchived(int? pageSize, int? page)
        {
            if (pageSize != null && page != null)
            {
                var approvalRequestsWithPagination = await approvalRequestsService.GetAllAsync(ar => ar.Status != AutopartStatus.WaitingForApproval, pageSize.Value, page.Value);
                return Ok(approvalRequestsWithPagination);
            }

            var approvalRequests = await approvalRequestsService.GetAllAsync(ar => ar.Status != AutopartStatus.WaitingForApproval);
            return Ok(approvalRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedApprovalRequestOutputModel>> Get(int id)
        {
            var approvalRequest = await approvalRequestsService.GetAsync(id);

            if (approvalRequest == null)
            {
                return NotFound(ApiResponse.Failure(ErrorMessages.ApprovalRequestNotFound));
            }

            return Ok(approvalRequest);
        }
    }
}
