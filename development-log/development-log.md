# Development log

Thoughts, opinions and decissions during the development process of the maintenance buddy app.



# start with the backend and create asp.net boilerplate api project
# create a little concept of the domain

![](images/2022-08-06-maintenance-domain-concept.excalidraw.png)

# strive for TDD and setup a test environment
- Unit-tests: 
  - should be possible
  - no much setup
- Integration test: 
  - want to test the whole api
  - therefore create a test-project that spins up the webserver and provides a client to test api calls