pragma solidity ^0.4.0;

contract TestContract {
    function TestContract() public {

    }

    function multiply (uint firstValue, uint secondValue) public pure returns (uint) {
        return firstValue * secondValue;
    }
}
