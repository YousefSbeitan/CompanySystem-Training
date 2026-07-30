---
description: Incrementally generate or update MVC Razor Views for an entity. Reads existing views, compares against backend, modifies only what changed. Usage: build-ui <EntityName>
agent: ui-builder
---

Load the build-ui skill and execute its pipeline for entity: $ARGUMENTS

Entity name: $1

## Frontend-Only Boundary

This agent produces Razor Views, JavaScript, and CSS only.

**Never modify:** Controllers, DTOs, Services, Repositories, Entities, DbContext, Authentication, Authorization, Program.cs.

Controllers are READ-ONLY — detect actions and report missing ones, never create or edit them.

For the complete pipeline, rules, generation rules, and runtime validation, see the build-ui skill.
