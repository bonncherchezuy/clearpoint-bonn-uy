import { fireEvent, render, screen } from '@testing-library/react'
import InputBox from './input-box.component'

describe('InputBox Tests', () => {
  describe('When rendering InputBox',() =>{
    let title; 
    let placeholder; 
    let type; 
    let value;
    let onChangeMock = jest.fn();

    beforeEach(() => {
      title = 'test title';
      placeholder = 'test placeholder';
      type = 'test type';
      value = 'test value';
      onChangeMock = jest.fn();

      render(
        <InputBox 
        title ={title}
        placeholder={placeholder}
        type={type}
        value={value}
        onChange={onChangeMock}      
       />
      );
    });

    it('should render the title text',() => {
      const actual = screen.getByText('test title');
      expect(actual).toBeInTheDocument()
    });

    it('should render the given type',() => {
      const inputDescription = screen.getByTestId('input-description');
      expect(inputDescription).toHaveAttribute('type','test type');
    });

    it('should render the placeholder text',() => {
      const inputDescription = screen.getByTestId('input-description');
      expect(inputDescription).toHaveAttribute('placeholder','test placeholder');
    });

    it('should render the value',() => {
      const inputDescription = screen.getByTestId('input-description');
      expect(inputDescription).toHaveAttribute('value','test value');
    });

    it('should call onchange handler on text change',() => {
      const inputDescription = screen.getByTestId('input-description');

      fireEvent.change(inputDescription, {target: {value: "test item x"}});

      expect(onChangeMock).toHaveBeenCalledTimes(1);
    });

  }); 
});