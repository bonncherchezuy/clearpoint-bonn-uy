import React from 'react';
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import { ItemsContext } from '../../contexts/items.context';
import TodoItem from './todo-item.component'

// Manually mock the dependency
jest.mock('axios', () => ({
  put: jest.fn(),
}));

import axios from 'axios';

describe('TodoItem Tests', () => {

  let itemContextMock;
  let item;

  beforeEach(() => {
    itemContextMock = {
      reloadItems: jest.fn(),
    };

    item = {
      id: '1111-0000',
      description: 'test description',
      isCompleted: true
    }

    render(
      <ItemsContext.Provider value={itemContextMock}>
        <TodoItem item={item}/>
      </ItemsContext.Provider>
      );
  });

  describe('When TodoItem is rendered', () => {
    it('should render the id',() =>{
      const actual = screen.getByText("1111-0000");
      expect(actual).toBeInTheDocument()
    });

    it('should render the description',() =>{
      const actual = screen.getByText('test description');
      expect(actual).toBeInTheDocument()
    });
  });

  describe('When Mark as completed button is click', () => {
    beforeEach(() => {
      axios.put.mockReturnValue(Promise.resolve('test ok'));
      itemContextMock.reloadItems.mockReturnValue('reload ok');
    });

    it('should call axios put command',() =>{
      const btnCompleted = screen.getByTestId("btn-completed");
      fireEvent.click(btnCompleted);

      expect(axios.put).toHaveBeenCalledTimes(1);
      expect(axios.put).toHaveBeenCalledWith(
      `${process.env.REACT_APP_BASEURI}/api/todoitems/${item.id}`,
      {
        id: '1111-0000',
        description: 'test description',
        iscompleted: true
      }
      );

    });

    it('should refresh item list', async() =>{
      const btnCompleted = screen.getByTestId("btn-completed");
      fireEvent.click(btnCompleted);
      await waitFor(() =>{
        expect(itemContextMock.reloadItems).toHaveBeenCalledTimes(1);
      });
    });
  });

});