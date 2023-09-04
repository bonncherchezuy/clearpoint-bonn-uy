import { Button, Container, Row, Form, Stack } from 'react-bootstrap'
import { useContext, useEffect, useState } from 'react';
import { ItemsContext } from '../../contexts/items.context';
import { ApiContext } from '../../contexts/api.context';
import axios from 'axios';
import InputBox from '../input-box/input-box.component';

const AddTodoContainer = () => {  

  const [inputDescription, setInputDescription] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const {items, reloadItems } = useContext(ItemsContext);
  const {uri} = useContext(ApiContext);

  useEffect(() => {
    setErrorMessage('');
  }, [items])

  const onAddButtonClick = async () => {
    await axios.post(uri,
    {
      description: inputDescription,
      isCompleted: false
    })
    .then((response) => 
    {
      setInputDescription('');
      reloadItems(uri);
    })
    .catch((error) => 
    {
      var data = error.response.data;
      var eMessage = typeof(data) === 'string' ? data : data.title
      setErrorMessage(eMessage)
      console.error(error);
    })
  }

  const onDescriptionChanged = (event) => {
    setInputDescription(event.target.value);
  }
  const onClearButtonClick = () =>{
    setInputDescription('')
    setErrorMessage('');
  }

  return (
    <Container data-testid="add-container">
      <h1>Add Item</h1>
      <span data-testid="error-msg" className="error">{errorMessage}</span>
        <InputBox 
        title ='Description'
        placeholder="Enter description..."
        type="text"
        value={inputDescription}
        onChange={onDescriptionChanged}      
       />
       
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button data-testid="add-btn" variant="primary" onClick={onAddButtonClick}>
            Add Item
          </Button>
          <Button data-testid="clear-btn" variant="secondary" onClick={onClearButtonClick}>
            Clear
          </Button>
        </Stack>
      </Form.Group>
    </Container>
  )
}

export default AddTodoContainer;