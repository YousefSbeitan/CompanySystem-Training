---
name: analyze
description: Deeply analyze a software project, identify the root cause of issues, trace execution flow, inspect architecture, and produce a structured engineering report without modifying code.
version: 1.0.0
author: Yousef
---

# Analyze Skill

## Purpose

The Analyze Skill is responsible for understanding a software project before making any changes.

Its responsibility is NOT to generate code immediately.

Instead, it behaves like a Senior Software Architect whose only mission is to discover the real problem, understand why it happens, collect evidence, and produce a complete technical report.

This skill must never blindly suggest changes.

Every conclusion must be supported by evidence found inside the project.

---

# Primary Responsibilities

The skill should:

- Analyze the complete project structure.
- Understand the architecture.
- Trace execution flow.
- Understand business logic.
- Detect inconsistencies.
- Detect security issues.
- Detect architecture violations.
- Detect incorrect dependency usage.
- Detect performance issues.
- Detect code smells.
- Detect duplicated logic.
- Detect broken design.
- Detect missing validations.
- Detect incorrect authorization.
- Detect incorrect authentication.
- Detect database inconsistencies.
- Detect mapping issues.
- Detect dependency injection issues.
- Detect lifecycle problems.
- Detect async issues.
- Detect exception handling problems.
- Detect API contract mismatches.
- Detect DTO inconsistencies.
- Detect Entity inconsistencies.
- Detect Repository misuse.
- Detect Service misuse.

The skill must never modify code.

Its job is analysis only.

---

# Thinking Process

The analysis process must always follow this exact order.

Never skip steps.

Never jump directly to a conclusion.

---

## Phase 1

Understand the project.

Questions:

- What framework is used?
- What language?
- Which architecture?
- Which design patterns?
- Which ORM?
- Which authentication mechanism?
- Which authorization mechanism?
- Which database?
- Which dependency injection style?
- Which folder organization?

Produce a short architecture summary.

---

## Phase 2

Understand the problem.

Never assume.

Read the issue carefully.

Extract:

- Expected behavior
- Actual behavior
- Error messages
- Stack traces
- Logs
- Screenshots
- User explanation

Create a Problem Statement.

---

## Phase 3

Locate affected modules.

Identify every file related to the issue.

For example:

Controller

↓

Service

↓

Repository

↓

Entity

↓

Mapper

↓

DTO

↓

Database

↓

Configuration

↓

Program.cs

↓

Middleware

↓

Authentication

↓

Authorization

↓

Helpers

↓

Extensions

↓

External services

Never stop at the first file.

Trace everything.

---

## Phase 4

Trace execution flow.

Follow every method call.

Example:

HTTP Request

↓

Controller

↓

Validation

↓

Service

↓

Repository

↓

Database

↓

Mapper

↓

Response

Every method must be verified.

Never assume the flow.

---

## Phase 5

Inspect every transformation.

Track data.

Example:

JSON

↓

DTO

↓

Mapper

↓

Entity

↓

Database

↓

Entity

↓

Mapper

↓

DTO

↓

JSON

Check:

- Missing fields
- Wrong mappings
- Null values
- Lost values
- Incorrect conversions
- Enum issues
- Type mismatches
- Formatting issues

---

## Phase 6

Validate business rules.

Questions:

Is the business logic correct?

Are all validations present?

Can invalid data reach the database?

Can unauthorized users execute the operation?

Can data be corrupted?

Can duplicated data exist?

Can deleted records still be used?

Can inactive users perform operations?

---

## Phase 7

Analyze authentication.

If authentication exists, inspect it completely.

Check:

- Authentication middleware
- JWT configuration
- Cookie configuration
- Identity configuration
- Authentication schemes
- Claims
- Token generation
- Token validation
- Refresh tokens
- Logout process
- Password hashing
- Password verification
- Expiration
- Signing keys
- Token rotation

Verify that:

- Tokens cannot be reused.
- Expired tokens are rejected.
- Revoked tokens are rejected.
- Passwords are never stored as plain text.
- Hash verification is correct.
- Refresh token rotation works correctly.
- Tokens contain the expected claims.

---

## Phase 8

Analyze authorization.

Inspect every endpoint.

Determine:

Who can access it?

Who should access it?

Check:

- AllowAnonymous
- Authorize
- Roles
- Policies
- Claims
- Ownership validation

Detect problems such as:

- Public endpoints that should be protected.
- Missing role checks.
- Missing ownership checks.
- Incorrect role names.
- Broken policy configuration.
- Insecure authorization flow.

---

## Phase 9

Analyze dependency injection.

Inspect:

Program.cs

Service registration

Repository registration

DbContext

Scoped services

Singleton services

Transient services

Detect:

- Missing registrations
- Wrong lifetime
- Circular dependencies
- Incorrect injections
- Unused services

---

## Phase 10

Analyze database consistency.

Inspect:

Entities

Relationships

Foreign keys

Indexes

Constraints

Soft delete

Tracking entities

Seed data

Migrations

Check:

- Missing foreign keys
- Broken relationships
- Incorrect navigation properties
- Nullable mistakes
- Incorrect cascade behavior
- Incorrect seed data
- Missing migrations
- Migration inconsistencies

---

## Phase 11

Analyze data flow.

Track every important variable.

Example:

Request

↓

DTO

↓

Validation

↓

Mapper

↓

Entity

↓

Repository

↓

Database

↓

Repository

↓

Mapper

↓

DTO

↓

Response

Verify that no information disappears during the flow.

Verify that values remain correct.

---

## Phase 12

Analyze exceptions.

Inspect:

try/catch

Custom exceptions

BusinessException

ResourceNotFoundException

Validation exceptions

Check:

- Swallowed exceptions
- Incorrect wrapping
- Missing logging
- Wrong HTTP status codes
- Hidden errors

Never recommend hiding exceptions.

---

## Root Cause Analysis

Never stop after finding one suspicious line.

Always continue searching.

The root cause must satisfy ALL of the following:

- Explains the observed behavior.
- Matches the execution flow.
- Matches the collected evidence.
- Does not contradict other evidence.
- Can be reproduced.

If multiple causes exist:

Rank them.

Example:

1. Very likely
2. Likely
3. Possible
4. Unlikely

Each cause must include supporting evidence.

Never guess.

---

## Confidence Score

Every conclusion must include a confidence level.

Example:

95%

80%

60%

40%

Explain why.

Higher confidence requires stronger evidence.

---

## Phase 13

Analyze the frontend.

The skill must never assume that the backend is the source of the issue.

Inspect the frontend if it exists.

Determine:

- Framework
- Routing
- State management
- API layer
- Authentication handling
- Authorization handling
- Forms
- Validation
- Error handling
- Local storage
- Session storage
- Cookies
- Token storage

Inspect every request.

Verify:

Request

↓

Headers

↓

Authorization

↓

Body

↓

URL

↓

HTTP Method

↓

Backend

Check for:

- Missing Authorization header
- Incorrect Bearer token
- Wrong endpoint
- Wrong HTTP method
- Wrong request body
- Missing fields
- Incorrect JSON
- Serialization problems
- Deserialization problems
- Missing refresh token
- Incorrect access token
- Expired token
- Wrong API URL
- Wrong environment configuration

Never assume the backend is responsible.

---

## Phase 14

Analyze API communication.

Inspect:

Frontend

↓

Network request

↓

Backend endpoint

↓

Controller

↓

Response

↓

Frontend rendering

Verify:

- Request reaches backend.
- Correct endpoint is called.
- Correct HTTP verb is used.
- Correct headers are sent.
- Correct response is returned.
- Correct status code is returned.
- Frontend correctly interprets the response.

Detect:

- CORS issues
- Serialization issues
- JSON mismatch
- DTO mismatch
- Property naming mismatch
- Missing fields
- Incorrect response parsing

---

## Phase 15

Collect evidence.

Every conclusion must include evidence.

Evidence examples:

- Source code
- Stack trace
- Runtime logs
- Console output
- SQL results
- Database records
- Network requests
- HTTP responses
- Configuration files
- Authentication claims

Never produce conclusions without evidence.

---

# Report Format

The final report must always contain the following sections.

---

## Executive Summary

Briefly explain:

- What was analyzed.
- Whether the issue was reproduced.
- Current project health.

---

## Problem Statement

Describe:

Expected behavior.

Actual behavior.

Observed symptoms.

---

## Architecture Overview

Summarize:

- Project architecture
- Layers
- Services
- Repositories
- Database
- Authentication
- Authorization

---

## Execution Flow

Describe the execution flow step by step.

Example:

HTTP Request

↓

Controller

↓

Validation

↓

Service

↓

Repository

↓

Database

↓

Repository

↓

Service

↓

Controller

↓

HTTP Response

---

## Findings

For every finding provide:

Title

Description

Evidence

Severity

Confidence

Affected files

Affected methods

Potential impact

---

## Root Cause

Explain:

Why the issue happens.

How it happens.

Where it happens.

When it happens.

Why it was not prevented.

---

## Recommended Fix

Do NOT modify code.

Instead explain:

- What should be changed.
- Why.
- Which files are affected.
- Possible side effects.
- Risks.

---

## Additional Observations

Include:

Security observations.

Architecture observations.

Performance observations.

Maintainability observations.

Future risks.

---

## Confidence

Provide an overall confidence percentage.

Example:

Overall confidence:

96%

Explain why.

---

# Rules

The skill MUST NEVER:

- Modify source code.
- Generate patches automatically.
- Hide errors.
- Ignore evidence.
- Guess.
- Skip execution flow.
- Stop after finding one issue.
- Assume the backend is responsible.
- Assume the frontend is responsible.
- Assume the database is responsible.
- Recommend changes without justification.

The skill MUST:

- Think like a Senior Software Architect.
- Verify every assumption.
- Trace the complete execution flow.
- Support every conclusion with evidence.
- Distinguish facts from hypotheses.
- Clearly indicate confidence levels.
- Produce a structured engineering report suitable for professional software teams.