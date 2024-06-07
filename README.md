# View Education Record API Integration Example code

The DfE have created a new service to enable learners to share their official education records with Further Education providers. 
To enable learners to share their record with providers, the software systems (Management Information Systems), that the providers use, must integrate with the ‘View Education Record APIs’ (VERA).

This repository includes all the code required to implement a simple Pattern A or Pattern B integration pattern.
It does not contain the required secrets to conect to the DfE test or production systems.

There are two integration patterns currently on offer. These are known as Pattern A and Pattern B and both are detailed below. 
Pattern A is slightly more technical work but should offer a much improved user experience for the user. 
Pattern B is offered in realisation that different MI vendors will work at different delivery speeds and their customers will require a mechanism to view this data if they cannot achieve that natively in their chosen MIS product.
These APIs are detailed in the following swagger link. https://api.sandbox.view-education-record.education.gov.uk/swagger/index.html 

## Pattern A
In this pattern the MIS calls VERA method “generate-qr-code”, this will reply with data that can be used to display a QR code that the learner can then scan directly from the DfE Education Record app. The learner scans the QR code which causes an authorisation record to be created so that calling a second method “learner-data” will return an up to date learner education record. The data specification and example JSON block is shown on the end of this document.
These two methods are secured and require a bearer token to authorise access. Each provider will be issued with a unique client id and client secret. The MI vendors will have to be able to setup these values in an appropriate settings database and use the values associated with the logged in user. Each client id is associated with a specific UKPRN. The learner will know which provider organisation is requesting the data and will see the name clearly displayed in the App.
The learner scanning the code sets up a relationship between the learner’s ULN and the users UKPRN – the learner is specifically authorising that provider to access their record. This pattern overcomes some of the issues that can be associated with the existing LRS ability to share flag (AKA privacy seen flag).

### Pattern A - Sequence
The college MIS system will have, in the context of a learner data entry screen:
1)	Generate QR Code button.
2)	This button will call into a new API – generate-qr-code. Secured by bearer token. The bearer token lets the DfE know the provider
3)	DFE payload reply with a QR Code + correlationid
4)	MIS System displays the QR Code
5)	Learner themselves scans the QR code and DfE gets a callback. DfE decode the claims, enhance with latest LRS data, DfE now have the data ready for presentation.
6)	MIS then calls ‘learner-data’ Parameters CorrelationId and secured by the same bearer token
7)	DfE return payload for learner. Or various error codes (see the swagger).

