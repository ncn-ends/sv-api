<img src="https://i.imgur.com/EnAmPfX.png" alt="Banner for repo">

test
# SV-API

The API portion of the StrategyVault project.  
For full repo and details about the project, see [here](https://github.com/ncn-ends/StrategyVault).




## Getting Started (Development)

### Configuration

1) Setup and configure https certs for dotnet
- If you are using linux, [here is a helpful script](https://github.com/ncn-ends/dotfiles/tree/master/create-dotnet-devcert).


2) Setup an SQL Server database for development and production
- For production, Azure SQL Database will be used.
- For development, you can use Azure SQL Database or setup a Docker container. For info on setting up the Docker container, [see here](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash).

3) Setup an Auth0 tenant for development and production.
- Full details and required scripts on setting up Auth0 are included in [sv-auth repo](https://github.com/ncn-ends/sv-auth) which is separate from this repo.

4) Setup configs for the application:
- For production, environment variables are used.
- For development, user secrets are used.

Example user secrets:
```json
{
  "ConnectionStrings": {
    "ApiDatabase": "SQLServerConnectionString"
  },
  "Auth0": {
    "Authority": "https://something.auth0.com/",
    "Audience": "custom-designed-audience",
    "ApiSecret": "rAnDoMStRiNgOfChaRs"
  }
}
```  

## Documentation

TODO: Need to provide link to documentation here
## Authors

- [@ncn-ends](https://www.github.com/ncn-ends)


## License

[MIT](https://choosealicense.com/licenses/mit/)

