# Dependency in your AutoMapper Profile constructor

Project to show you how you can use dependency injection in your `Profile` classes.

Of course, it's best to use services for this, but if you really, really need to, you can also use constructor injection in a `Profile`.

It comes down to creating an instance of your `Profile` and providing it to the configuration's `AddProfile` method. This way the IoC-container will be responsible for resolving an instance of the class, which means the IoC-container will also resolve all necessary dependencies.

Just remember: **just because you can, doesn't mean you should**!