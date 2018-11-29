**Documentation**
Iframe documentation:  http://docs.tokenex.com/#tokenization-iframe
Payment Services API:  http://docs.tokenex.com/#tokenex-api-payment-services-process-transaction
Litle and Co Specific Docs:  http://docs.tokenex.com/#appendix-gateway-parameters-litle-co

**Test Data and credentials**
TokenEx ID: 7639326364821993
API Key: QRRMSOUU6q7od62bqcoW
Iframe Client Secret: QRRMSOUU6q7od62bqcoW
Test Credit Card Number: 4242424242424242 or 5454545454545454
Expiration Date: any date in the future
CVV: 123 or 456 or 789



*   implement API call here http://docs.tokenex.com/#tokenex-api-payment-services-process-transaction
*  use Litle & Co gateway  http://docs.tokenex.com/#appendix-gateway-parameters-litle-co
{
 
"APIKey": "<<insert TokenEx ID>>",
"TokenExID": "<<insert TokenEx ID>>",
"TransactionType": 1,
"TransactionRequest": {
	"gateway": {
		"name": "LitleGateway",
		"merchant_id": "TokenEx",
		"login": "TokenEx",
		"password": "TokenEx"
	},
	"credit_card": {
		"number": "<<Insert Token>>",
		"month": "<<Insert exp month>>",
		"year": "<<Insert exp year>",
		"verification_value": "<<insert CVV>>",
		"first_name": "<<insert name>>",
		"last_name": "<<insert name>>"
	},
	"transaction": {
		"amount": 1000
	}
}
}