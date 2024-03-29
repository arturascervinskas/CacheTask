# CashTask
Create web api using latest dotnet LTS version, where user could store key-value pairs. In other words - a dictionary.
key - string
value - List<object>

Required endpoints:
1. create (if the key exists - value should be overriden)
2. append (if key does not exist - it should be created)
3. delete (by the key)
4. get (return List<object> based on a given key)


Additional requirements:
1. created values should have expiration period that should be received during 'create' call. If expiration period is not defined - default value should be used from config
2. each time after 'get' is used for a particular key, the expiration period for that key should reset (meaning that only unused values are expiring)
3. dictionary should have it's internal cleanup mechanism to get rid of "zombie" records and maintain health memory usage by cleaning up the trash. Cleanup interval and max expiration period should be defined in config. User should not be allowed to create records with expiration period longer that the max defined in config.
4. api should have swagger documentation and it should be the starting point when launching the app. Don't add any additional endpoint description - just plain minimalistic swagger setup for a basic usage.
It is known that the resulting source code will be given to another team with less experiences developers for further extending and maintenance. There are rumors floating that the API will get even more responsibilities so it's very likely that complexity will grow. You are the person setting up the project and it is expected that you show the best coding practices (REST standards, SOLID principles, project structure and so on)


Optional
Maintain stored data after API is restarted
Add authorization (for example api-key)


The source code should be put into some source openly accessible source control system like GitHub, BitBucket, GitLab or any other.
