Feature: ApiTests	

Scenario: None	
Given path "api/reminder"
And query string "amount" = "0"
When I "GET"
Then the response body should be 
| Days | Message |

Scenario: One
Given path "api/reminder"
And query string "amount" = "1"
When I "GET"
Then the response body should be 
| Days | Message |
| 2    | Hello   |

Scenario: Two
Given path "api/reminder"
And query string "amount" = "2"
When I "GET"
Then the response body should be 
| Days | Message |
| 2    | Hello   |
| 3    | Goodbye |

Scenario: Post
Given path "api/reminder"
And body
| Days | Message |
| 1    | New     |
When I "POST"
Then the response code is "200"
