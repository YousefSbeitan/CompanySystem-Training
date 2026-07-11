---
description: Deeply analyze the current project or a specific issue and produce a complete engineering report without modifying any code.
---

# Analyze Command

Your task is to deeply analyze the current software project or the issue described by the user.

Use the **Analyze Skill**.

Your goal is NOT to generate code.

Your goal is NOT to modify code.

Your goal is to discover the real root cause of the issue and produce a complete engineering report.

---

## Responsibilities

You must:

- Understand the user's request.
- Understand the project architecture.
- Analyze the complete execution flow.
- Inspect all affected layers.
- Collect evidence.
- Produce a structured report.
- Explain the root cause.
- Recommend a fix without implementing it.

---

## Scope

Analyze every relevant layer.

This includes (when applicable):

- Frontend
- Backend
- Controllers
- Services
- Repositories
- Entities
- DTOs
- Mappers
- Middleware
- Authentication
- Authorization
- Dependency Injection
- Configuration
- Database
- Migrations
- API Contracts
- External Services
- Logging
- Validation

Never assume the problem belongs to a specific layer.

---

## Analysis Strategy

Follow this order.

### 1. Understand the Problem

Determine:

- Expected behavior
- Actual behavior
- Error messages
- Reproduction steps
- Environment
- User observations

If important information is missing, explicitly state it.

---

### 2. Understand the Architecture

Identify:

- Framework
- Language
- Project structure
- Layers
- Authentication method
- Authorization method
- Database
- Design patterns

Provide a concise architecture summary.

---

### 3. Trace the Execution Flow

Trace the request completely.

Example:

HTTP Request

↓

Controller

↓

Validation

↓

Business Layer

↓

Repository

↓

Database

↓

Repository

↓

Business Layer

↓

Controller

↓

HTTP Response

Never stop tracing until the complete flow is understood.

---

### 4. Inspect Data Flow

Track important data through the application.

Examples:

- UserId
- Tokens
- Passwords
- Claims
- DTOs
- Entities
- Mapped objects

Verify that values remain correct throughout the flow.

---

### 5. Analyze Security

Inspect:

Authentication

Authorization

Ownership validation

Role validation

Claims

JWT

Refresh Tokens

Password hashing

Password verification

Session handling

Token lifecycle

Look for:

- Missing authorization
- Missing ownership validation
- Token misuse
- Password issues
- Incorrect claims
- Privilege escalation
- Security vulnerabilities

---

### 6. Analyze Frontend (if present)

Inspect:

- Authentication flow
- Authorization flow
- API requests
- Request payloads
- Response handling
- Local storage
- Session storage
- Cookies
- Routing
- State management

Verify that the frontend communicates correctly with the backend.

Do not assume the backend is responsible.

---

### 7. Analyze Database

Inspect:

- Tables
- Relationships
- Constraints
- Foreign Keys
- Seed Data
- Migrations
- Stored values

Verify consistency between:

Database

↓

Entities

↓

Repositories

↓

Services

---

### 8. Collect Evidence

Every finding must include evidence.

Evidence may include:

- Source code
- Runtime behavior
- SQL data
- Logs
- HTTP requests
- HTTP responses
- Stack traces
- Configuration
- Screenshots

Never make unsupported claims.

---

### 9. Determine the Root Cause

Do not stop after finding one suspicious line.

Continue until the true root cause is identified.

If multiple causes exist:

Rank them by likelihood.

Explain why.

---

### 10. Recommend a Solution

Do NOT implement the fix.

Instead explain:

- What should be changed.
- Why it should be changed.
- Which files are affected.
- Risks.
- Side effects.
- Alternative solutions.

---

# Report Format

The final report must contain:

## Executive Summary

Short overview of the issue and current system state.

---

## Problem Statement

Expected behavior.

Actual behavior.

Symptoms.

---

## Architecture Overview

Relevant architecture summary.

---

## Execution Flow

Complete request flow.

---

## Findings

For every finding include:

- Title
- Description
- Evidence
- Severity
- Confidence
- Affected files
- Affected methods

---

## Root Cause

Explain:

- Why it happens.
- Where it happens.
- How it happens.

---

## Recommended Fix

Describe the required changes without modifying the code.

---

## Risks

Describe:

- Current risks
- Future risks
- Security risks
- Performance risks

---

## Overall Confidence

Provide a confidence percentage.

Explain why.

---

# Rules

You MUST NOT:

- Modify source code.
- Generate patches automatically.
- Guess.
- Skip execution flow.
- Skip evidence.
- Ignore alternative explanations.
- Assume the backend is responsible.
- Assume the frontend is responsible.
- Assume the database is responsible.

You MUST:

- Think like a Senior Software Architect.
- Analyze before concluding.
- Support every conclusion with evidence.
- Distinguish facts from hypotheses.
- Produce a professional engineering report.
- Clearly indicate uncertainty when evidence is insufficient.