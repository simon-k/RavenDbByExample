# RavenDbByExample
A short demonstration of how RavenDB can be used to persist data

# How it works
- Simply reference RavenDB from NuGet.
- Query by linq

# The Niec Parts
- No SQL!
- No Stored Procedures!
- No need for a DBA to make a schema change
- Testable

# Tricky Parts
- Eventual concistency
- Indexing becomes a bit complex if relations are used between collections
- Number of returned documents are default 128. Max 1024. So pagination is needed
- Max number of operations in a session is 20.

# TODO
- Revisions.