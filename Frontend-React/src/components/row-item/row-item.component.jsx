import { Row, Col} from 'react-bootstrap'

const RowItem = ({children}) => (
  <Row>
    <Col>
      {children}
    </Col>
  </Row>
);

export default RowItem