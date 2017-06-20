# RavenDbByExample
A short demonstration of how RavenDB can be used to persist data

# How it works
- Simply reference RavenDB from NuGet
- Create a document store
- Open session

# The Niec Parts
- No SQL!
- No Stored Procedures!
- No need for a DBA to make a schema change!
- No need to coordinate deployments with DBA!
- Testable!
- Lucene / Linq queries

# Tricky Parts
- Eventual concistency
- Indexing becomes a bit complex if relations are used between collections
- Number of returned documents are default 128. Max 1024. So pagination is needed
- Max number of operations in a session is 20.

# Note
- The console app requires ravendb running locally on port 8080, and versioning enabled.

# TODO
- Test Revisions