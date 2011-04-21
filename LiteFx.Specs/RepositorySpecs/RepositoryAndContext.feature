﻿Feature: Working on a Context trough a Repository
	In order to use the repository pattern
	and make my software rocks with this pattern
	As a developer
	I want to manipulate a ORM context trough a Repository

Scenario: Selecting an entity by it's id
	Given a Context
	And a Repository
	When I call the GetById method using the valid id 1
	Then a entity instance with the id 1 should be returned

Scenario: Selecting all entities
	Given a Context
	And a Repository
	When I call the GetAll method
	Then a entity collection should be returned