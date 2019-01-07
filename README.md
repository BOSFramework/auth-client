# BOS Auth API Client

This client is intended to make working with the BOS Auth API easier, and to abstract away the need to construct query strings for most if not all scenarios. It also attempts to make working with errors and problems easier as well.

## Response Objects

 Each method returns an object that ends with the suffix "Response". These Response Objects always present the user with the ability to see the status code that was returned, and to perform a IsSuccesStatusCode check as well. In Addition, certain response objects that represent the return values of methods where there may have been multiple types of errors that could have occured have a list of AuthError objects on them as well. These AuthErrors will contain a code and a message that informs the user of what the problem may have been if there was one. In addition to these things there is also a property that represents the object requested if there was a value to be returned. Ex: a list of users from a GetUsers request would be on the response object as 'Users'.

### AuthError Codes Used

 101 - Indicates that the basic form of the data was bad and thus resulted in a bad request.

 111 - Indicates a verification attempt failure due to the provided password not matching the one on record.

 409 - Conflict, this is the same code as the HTTP Response for a conflict. Used to represent that something has been attempted to be added that conflicts with another existing record.

### Using the Client Library

 In order to use the Client Library, you must first include it in your project. Currently, this is done through creating a submodule. Create a submodule folder at the root alongside the src folder. Then inside of it run : 

 ```bash
git submodule <repoClonePath>
 ```

 Once you have it submoduled, check out the branch you need and run a git fetch/pull.

 From there you simply add the project reference to the project that will be using the Client Library. Once you have it all set up, you will add the client library to your HttpClientFactory through the ConfigureServices method of the Startup.cs class with the following code:

```csharp
services.AddBOSAuthClient("passInBOSApiKey");
```

From there you can consume the services by injecting the client into wherever you need it through constructor injection:

```csharp
public class SomeClass
{
    private readonly _authClient;

    SomeClass(IAuthClient authClient)
    {
        _authClient = authClient;
    }
}
```

In order to leverage Odata's open types, many methods are generic, and they require you to create you own implementation of the interface they require. This allows you to be able to specify the properties that you want on the object that you are sending. You can also choose not to extend the class and to simply implement the interface. Once you have a concrete implementation, you just pass it in and it will return to you the types that you defined.

```csharp
public class MyUser : IUser
{
    // implement all of IPerson's properties
    // ...
    // ...

    public string MyExtraProperty { get; set; }
}

// Create a new user and then extend the properties of you custom user onto it.
UserResponse<MyUser> myNewRoleResponse = _authClient.AddNewUserAsync<MyUser>("username", "email", "password");
ExtendUserResponse<MyUser> extendUserResponse = _authClient.ExtendUserAsync(myNewUserResponse.User);
```

### Important Notes

There is currently no way to extend the role class. This will be added when the functionality is available on the API.

The only current properties that can be used for extensions are string and ints. Any complex type seems to cause problems with Odata and will need further attention to make more robust.
