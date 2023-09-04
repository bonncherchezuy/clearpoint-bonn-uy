import { Button } from 'react-bootstrap'
import { useContext } from 'react';
import { ItemsContext } from '../../contexts/items.context';
import { ApiContext } from '../../contexts/api.context';
import axios from 'axios';

const TodoItem = ({item}) => 
{
  const {reloadItems } = useContext(ItemsContext);
  const {uri} = useContext(ApiContext);

  const onMarkAsCompleteClick =  async (item) => {
    try {
    await axios.put(`${uri}/${item.id}`, 
      { id: item.id,
        description: item.description,
        iscompleted: true});
        reloadItems(uri);
    } catch (error) {
      console.error(error)
    }
  }

  const {id, description} = item
  return(<tr key={id}>
    <td>{id}</td>
    <td>{description}</td>
         <td>
           <Button data-testid="btn-completed" variant="warning" size="sm" onClick={() =>{onMarkAsCompleteClick(item)}}>
             Mark as completed
           </Button>
        </td>
  </tr>);
};

export default TodoItem