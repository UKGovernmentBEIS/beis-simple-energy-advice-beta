using SeaPublicWebsite.BusinessLogic;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Services;

namespace SeaPublicWebsite.ManagementShell;

public class CommandHandler(IOutputProvider outputProvider, IDataAccessProvider dataAccessProvider)
{
    public async Task TrySetMaintenanceState(string[] args)
    {
        var emergencyMaintenanceService = new EmergencyMaintenanceService(dataAccessProvider);

        DisplayMaintenanceStateWarnings();

        var argEmergencyMaintenanceState = ParseEmergencyMaintenanceStateInput(args);
        if (argEmergencyMaintenanceState is null) return;

        var argEmergencyMaintenanceVerb =
            argEmergencyMaintenanceState == EmergencyMaintenanceState.Enabled ? "ENABLE" : "DISABLE";
        var liveEmergencyMaintenanceState = await emergencyMaintenanceService.GetEmergencyMaintenanceState();

        DisplayMaintenanceStateDetails(argEmergencyMaintenanceVerb, liveEmergencyMaintenanceState);

        if (argEmergencyMaintenanceState == liveEmergencyMaintenanceState)
        {
            outputProvider.Output("The requested state is the same as the current state.");
            outputProvider.Output("Exiting without changes...");
            return;
        }

        var confirmation = GetUserConfirmationForSettingMaintenanceState(argEmergencyMaintenanceVerb);
        if (!confirmation) return;

        var authorEmail = GetUserEmailForAudit();
        if (authorEmail is null) return;

        await emergencyMaintenanceService.SetEmergencyMaintenanceState(
            (EmergencyMaintenanceState)argEmergencyMaintenanceState, authorEmail);

        outputProvider.Output("Output Complete.");
    }

    private string? GetUserEmailForAudit()
    {
        var authorEmail = outputProvider.GetString("Please enter your email address for the audit record:");

        if (string.IsNullOrWhiteSpace(authorEmail))
        {
            outputProvider.Output("No email address entered.");
            outputProvider.Output("Exiting without changes...");
            return null;
        }

        return authorEmail;
    }

    private bool GetUserConfirmationForSettingMaintenanceState(string emergencyMaintenanceVerb)
    {
        outputProvider.Output("!!!!!!!!!!!!!!!!!!!!!!");
        outputProvider.Output(
            $"Please confirm you would like to {emergencyMaintenanceVerb} emergency maintenance mode.");
        var confirmation = outputProvider.Confirm("Would you like to continue? (Y/N)");

        if (!confirmation)
        {
            outputProvider.Output("Exiting without changes...");
        }

        return confirmation;
    }

    private void DisplayMaintenanceStateDetails(string emergencyMaintenanceVerb,
        EmergencyMaintenanceState liveEmergencyMaintenanceState)
    {
        var isMaintenanceStateEnabled = liveEmergencyMaintenanceState == EmergencyMaintenanceState.Enabled;

        outputProvider.Output("Details:");
        outputProvider.Output($"Request is to {emergencyMaintenanceVerb} emergency maintenance mode.");
        outputProvider.Output(
            $"Portal emergency maintenance mode is currently {(isMaintenanceStateEnabled ? "ENABLED" : "DISABLED")}.");
        if (isMaintenanceStateEnabled)
        {
            outputProvider.Output("Recommendations cannot be viewed.");
        }
    }

    private void DisplayMaintenanceStateWarnings()
    {
        outputProvider.Output("!!!!!!!!!!!!!!!!!!!!!!");
        outputProvider.Output("This is a dangerous operation! Read the following before proceeding:");
        outputProvider.Output("This function will enable or disable emergency maintenance mode.");
        outputProvider.Output(
            "In emergency maintenance mode, all requests to this portal will be blocked with a 503 response.");
        outputProvider.Output(
            "This should only be used in accordance with our disaster response plan when usage of the site by members of the public needs to be blocked.");
        outputProvider.Output("If unsure, quit and consult the documentation.");
        outputProvider.Output("!!!!!!!!!!!!!!!!!!!!!!");
    }

    private EmergencyMaintenanceState? ParseEmergencyMaintenanceStateInput(string[] args)
    {
        try
        {
            return Enum.Parse<EmergencyMaintenanceState>(args[0].Trim(), true);
        }
        catch (Exception e) when (e is ArgumentException or IndexOutOfRangeException)
        {
            var allSubcommands = string.Join("/", Enum.GetValues<EmergencyMaintenanceState>());
            outputProvider.Output(
                $"Please specify whether to enable or disable emergency mode - Usage: SetEmergencyMaintenanceState <{allSubcommands}>");
            outputProvider.Output("Exiting without changes...");
            return null;
        }
    }
}