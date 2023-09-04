import {Alert} from 'react-bootstrap'


const BulletinBoard = ({body}) =>
{
  const {title, description, notes} = body;
  return(
  <Alert data-testid="bulletin-board" variant="success">
    <Alert.Heading>{title}</Alert.Heading>
    {description}
    <br />
    <br />
    <ol className="list-left">
      {notes.map((note) => {
        return(
        <li key={note.id}>
          {note.message}
        </li>);
      }
      )}
    </ol>
  </Alert>
  );
}

export default BulletinBoard