# Logging

## Logit.io

GovPaaS has instructions on how to export console logs to Logit.io. However, the ASP.Net Core default console logging is not very good so the default GovPaaS solution doesn't work very well.

Logit.io have instructions on their site on how to use Serilog instead. This is what we are using on this project along with a Serilog network sink to push the logs to Logit.io.

We have a single alerting rule configured on Logit.io to alert us every time a Critical or Error log is raised in the website code.
