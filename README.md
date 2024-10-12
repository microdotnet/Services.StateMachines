# MicroDotNet - StateMachines

Microservice that provides state machines management and execution capabilities.

## Table of contents
- [Instalation](#installation)

## Installation

### Steps

1. Clone the repository
	```bash
	git clone https://github.comm/MicroDotNet/Services.StateMachines.git
	```

1. Open solution `Services.StateMachines.sln`.

1. Restore packages

1. Build and run solution

### ELK stack preparation

1. Prepare a service token to allow communication between Kibana and ElasticSearch

     1. Run `docker compose up -d`.
	 1. On ElasticSearch container run `bin/elasticsearch-service-tokens create elastic/kibana kibana-token`.
	 1. Get the result from the prompt and paste into `.\docker\kibana\kibana-template.yml` file as value for `elasticsearch.serviceAccountToken` variable.
	 1. Stop the stack with `docker compose down`
	 1. Rename `\docker\kibana\kibana-template.yml` to `\docker\kibana\kibana.yml`.
	 1. Start the stack again.