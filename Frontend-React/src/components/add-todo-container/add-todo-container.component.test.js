import React from 'react';
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import { ItemsContext } from '../../contexts/items.context';
import AddTodoContainer from './add-todo-container.component'

// Manually mock the dependency
jest.mock('axios', () => ({
  post: jest.fn(),
  get: jest.fn(),
}));

import axios from 'axios';

describe('AddTodoContainer Tests', () => {
  let itemContextMock;

  beforeEach(() => {
    itemContextMock = {
      items: [],
      setItems: jest.fn(),
      reloadItems: jest.fn(),
    };

    render(
    <ItemsContext.Provider value={itemContextMock}>
      <AddTodoContainer/>
    </ItemsContext.Provider>
    );
  })
  
  describe('when item is added', () => {

    beforeEach(() => {
      axios.post.mockReturnValue(Promise.resolve('test ok'));
      itemContextMock.reloadItems.mockReturnValue('reload ok');
    });

    it('should get value from description and add to context', () => {

      const inputDescription = screen.getByTestId("input-description");
      const addButton = screen.getByTestId("add-btn");
      
      fireEvent.change(inputDescription, {target: {value: "test item x"}});
      fireEvent.click(addButton);

      expect(axios.post).toHaveBeenCalledTimes(1);
      expect(axios.post).toHaveBeenCalledWith(
      `${process.env.REACT_APP_BASEURI}/api/todoitems`,
      {
        description: 'test item x',
        isCompleted: false,
      }
      );
    });

    it('should clear the description in the input box', async () => {

      const inputDescription = screen.getByTestId("input-description");
      const addButton = screen.getByTestId("add-btn");

      fireEvent.change(inputDescription, {target: {value: "test item x"}});
      fireEvent.click(addButton);

      await waitFor(() =>{
        expect(itemContextMock.reloadItems).toHaveBeenCalledTimes(1);
        expect(inputDescription).toHaveAttribute('value','');
      });

    });

    it('should the refresh the items ', async () => {

      const inputDescription = screen.getByTestId("input-description");
      const addButton = screen.getByTestId("add-btn");

      fireEvent.change(inputDescription, {target: {value: "test item x"}});
      fireEvent.click(addButton);

      await waitFor(() =>{
        expect(itemContextMock.reloadItems).toHaveBeenCalledTimes(1);
      });

    });

  });

  describe('when clear button is click', () => {

    axios.post.mockReturnValue(Promise.resolve('test ok'));

    it('should clear the description in the input box', () => {
      const inputDescription = screen.getByTestId("input-description");
      const clearBtn = screen.getByTestId("clear-btn");

      fireEvent.change(inputDescription, {target: {value: "test item x"}});
      
      //making sure that inputDescription has value before clear button click
      expect(inputDescription).toHaveAttribute('value','test item x');

      //trigger clear button
      fireEvent.click(clearBtn);

      //expect that value should be cleared
      expect(inputDescription).toHaveAttribute('value','');

    });
  });

});

