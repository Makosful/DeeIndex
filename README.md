# DeepIndex

DeepIndex is a search engine school project, a learning experience rather than
a practical solution.
The practical aim of this project is to learn to scale projects.

The architecture aims to extract every service into a microservice and containerize them, to allows for scaling.

## Ports

Project      | HTTP | HTTPS |
---          | ---  | ---   |
LoadBalancer | 5000 | 5001  |
Indexer      | 5010 | 5011  |