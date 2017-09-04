# Cranus

The Cranus is an In-Memory implementation of a typical CQRS/Event Sourcing flow, using the Cronus framework.
The aim of this project is to show how the most basic functionalities of the Cronus work, while avoiding any concers about persisting mechanisms and such.
## Installation

+ Simply Fork the project.

## Usage

An educational entry-level guide to display how you can implement CQRS/Event Sourcing in your projects. 

## Basic Concepts

+ **ICommand**
  + A command is what causes an Aggregate to change. However, the commands do not directly interact with  
    the aggregates. 
+ **IEvent**
  + An event is raised when a command which passed and succeeded in changing the aggregate.  
+ **ApplicationService**
  + An applicationService handles commands and sends them to the AggregateRoot in order to make the changes happen.
+ **IProjection**
    + A projection is simply a way to show specific information of an aggregate. You can model a DTO in order to  
     store the information needed and simply populate it in the projection.
+ **IPort**
    + A Port establishes a connection between two or more aggregates, even in different Bounded Contexts. You  
     can use it to issue commands, concerning various aggregates, after a certain operation is finished to another  
     aggregate.
+ **State** 
    + The state is mostly self-explanatory, it contains the information about the current state of an aggregate. You can  
      imagine this as the properties of the aggregate.

## Basic Flow 

1. A **Command** is issued.
2. An **AppService** handles the command for the given aggregate which it affects.
3. The **AppService** delegates the command to the **AggregateRoot**.
4. The **AggregateRoot** calls a method and Validates the information passed.
5. After the validation, an **Event** is raised which changes the current **State** of the aggregate.

+ *Optional*
  + You can make a **Projection** to display needed business logic information by implementing the **IProjection**              interface on a class and attach **EventHandlers** to it.
  + You can make a **Port** to connect two or more different contexts by implementing the IPort interface on a  
    class and attach **EventHandlers** to it.

**For More Information** Go to [Elders/Cronus.](https://github.com/Elders/Cronus)

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request.