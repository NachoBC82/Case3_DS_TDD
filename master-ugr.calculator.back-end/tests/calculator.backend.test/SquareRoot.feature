Feature: SquareRoot
	As Alice (the customer)
	I want to calculate the
	square root of a number

Scenario: A number positive
	Given the number is 4
	When the square root is calculated
	Then the square root result should be 2

Scenario: A number negative
	Given the number is -4
	When the square root is calculated
	Then the square root result should be NaN

Scenario: A number zero
	Given the number is 0
	When the square root is calculated
	Then the square root result should be 0