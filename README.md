# Fleeting
### Handle fleeting async errors 
Fleeting is simple libary designed to assist with retry and async.  While there are other similar libraries out there, we have found that they have not kept up, especially with async.

## Installation

Fleeting is on nuget:

[https://www.nuget.org/packages/Fleeting](https://www.nuget.org/packages/Fleeting)

    Install-Package Fleeting -Pre 

## Basic Usage

Full documentation is at [our wiki](https://github.com/cleverguy25/Fleeting/wiki)

#### Basic Retry

    var retryPolicy = new RetryPolicy(exception => exception is TimeoutException, 5, RetryIntervalFactory.GetFixedInterval(100));
    var result = await retryPolicy.ExecuteAsyncWithRetry(async () => await SomeAsyncStuff());

What is happening?  The first parameter to RetryPolicy is how we determine if it is a transient error.  The second is the number of retries before giving up, and the last is a function that determines the time span to wait givent the current retry count (so you can use the built in fixed, linear, and exponential intervals, or make up your own logic).

Here is an example with Sql Extension methods using the default Sql Retry Policy:

    using (var connection = new SqlConnection("connection string"))
    {
        using (var command = new SqlCommand("command text", connection))
        {
            await connection.OpenAsyncWithRetry();
            await command.ExecuteNonQueryAsyncWithRetry();
        }
    }
There are extensions for most of the SqlCommand.Execute methods.

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D
