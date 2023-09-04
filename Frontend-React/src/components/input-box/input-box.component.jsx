import { Row, Col, Form} from 'react-bootstrap'

const InputBox = ({title, placeholder, type, value, onChange}) =>
{
return(
  <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
    <Form.Label column sm="2">
      {title}
    </Form.Label>
    <Col md="6">
      <Form.Control
        data-testid="input-description"
        type={type}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
      />
    </Col>
  </Form.Group>
)};

export default InputBox