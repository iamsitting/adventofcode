package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func solvePart1(input string) int {
	lines := strings.Split(input, "\n")
	numbers := []int{}
	for _, line := range lines {
		var first byte = 0
		var last byte = 0
		for _, ch := range line {
			if '1' <= ch && ch <= '9' {
				if first == 0 {
					first = byte(ch)
				}
				last = byte(ch)
			}
		}

		if first != 0 && last != 0 {
			num, err := strconv.Atoi(string([]rune{rune(first), rune(last)}))
			if err == nil {
				numbers = append(numbers, num)
			}
		}
	}

	sum := 0
	for _, num := range numbers {
		sum = sum + num
	}
	return sum
}

func testPart1() {
	input := `
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
  `

	result := solvePart1(input)
	if result == 142 {
		fmt.Println("Test passed.")
	} else {
		fmt.Println("Test failed got %s, expected 142", result)
	}
}

func getAnswerPart1() {
	content, err := os.ReadFile("../inputs/day.txt")
	if err != nil {
		panic("day.txt could not be read")
	}

	input := string(content)
	result := solvePart1(input)
	fmt.Println("Answer: ", result)
}

func main() {
	getAnswerPart1()
}
