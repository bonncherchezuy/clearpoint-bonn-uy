import { Button, Table, } from 'react-bootstrap'
import { useContext, useEffect } from 'react';
import { ItemsContext } from '../../contexts/items.context';
import { ApiContext } from '../../contexts/api.context';
import TodoItem from '../todo-item/todo-item.component'
import axios from 'axios';

const TodoItemsList = () => {
  const {items, setItems, reloadItems } = useContext(ItemsContext);
  const {uri} = useContext(ApiContext);

  useEffect(() => {
    fetchInitialData();
  }, [])

   const fetchInitialData = async () => 
   {
    await axios(uri)
    .then((response) => {setItems(response.data)})
    .catch((error) => {
      console.error(error);
    })
   }
   const onRefreshButtonClick = async () => {
     await reloadItems(uri);
   }

  return (
    <>
      <h1>
        Showing {items.length} Item(s)
        <Button data-testid="btn-refresh" variant="primary" className="pull-right" onClick={onRefreshButtonClick}>
          Refresh
        </Button>
      </h1>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <TodoItem key={item.id} item={item} />
          ))}
        </tbody>
      </Table>
    </>
  )
}

export default TodoItemsList