This is a short how to for those who want to try out this plugin. It`s necessary to remember that this piece of software is currently
in it`s early development stage, so some feautures may be a litte buggy. Also it`s is not completely implemented. A list of the already
implemented feautures can be found in the log below:

Currently implemented feautures:

- Complete UDP connection classes, including sending and recieving broadcast messages for all hosts in the same network.
- First state of the protocol completely implemented and functional (hosts are able to see each other in the network, for more details
will be found on the project's monography)

How to use:

- Create an empty GameObject and atach to it a component called P2PNetworking. Don't forget to set a port for the broadcast channel in 
unity's inspector.
- As default, the plugin will start in StandBy Mode. Which means that it will be already listening to other peers in the network. 
- To get a full list of all the found peers just access the public get attribute knownHosts. It is accessible on the P2PNetworking 
active instance. To make access to this instance easily, just call the static public attribute P2PNetworking.instance . The list will
be allways updated while the compoment is active and the mode is set to StandBy.
