# MessageBus
Simple implementation of publish/subscribe pattern in C#.

## How to use?

### Subscribing for the message

````c#
IMessageBus bus = new MessageBus();
bus.Subscribe<MyMesssageOrEvent>(m => { /* Handle the message here... */ });
````
### Publishing messages

````c#
IMessageBus bus = new MessageBus();
bus.Publish(new MyMesssageOrEvent());
````
## TODO

- [ ] Implement _IObserver<T>_ and  _IObservable<T>_ interfaces (if makes sense)