# DIYVCV MVP
DIYVCV stands for Do It Yourself Virtual Control Voltage. Virtual control voltage refers to modular synthesizers/electronic music instruments.
This is a database that helps hobbyists pick a project they like and find electronic components needed to build that project at home. 

This is an example of an MVP that will Create, Read, Update and Delete from a database. The relationships can be modified through the web interface.

![Home Screen](https://github.com/christkinsman/DIYVCV/assets/home.png)

![List Modules](https://github.com/christkinsman/DIYVCV/blob/master/DIYVCV/assets/home.png)
![Show Module](https://github.com/christkinsman/DIYVCV/blob/master/DIYVCV/assets/show-module.png)
![Update Module](https://github.com/christkinsman/DIYVCV/blob/master/DIYVCV/assets/update-module.png)


## To Run This Project
- Clone Codebase
- **Right Click Project > View Project in File Explorer > Create Folder "App_Data"**
- Tools > Nuget Package Manager > Package Manager Console
- enable-migrations
- add-migration {migration_name}
- update-database
- update portnumber in PlayerController.cs, TeamController.cs, SponsorController.cs

## Common Issues
### Unable to access part of path ... roslyn/csc.exe
- Right click solution > Clean
- Right click solution > Build
- Right click solution > Rebuild

### Update Database failed. Could not create .mdf file
- Create "App_Data" Folder in project

### The name or namespace 'Script'/'JavaScriptSerializer' could not be found
- Right click "References"
- Add System.Web.Extensions

### Every Action says something went wrong
- Update the port number to the current port your local project is using
- update portnumber defined in PlayerController.cs, TeamController.cs, SponsorController.cs

### Adding a player leads to an error
- Add a team first, each player must play for a team

References
- [Previous Project PetGrooming](https://github.com/christinebittle/PetGroomingMVC)
- [Previous Project BlogProject 7th iteration](https://github.com/christinebittle/BlogProject_7)
- [Utilized Lightbox plugin by Lokesh Dhakar](https://lokeshdhakar.com/projects/lightbox2/)
