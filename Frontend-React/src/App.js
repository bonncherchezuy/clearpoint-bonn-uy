import './App.css'
import { Image, Container } from 'react-bootstrap'
import React from 'react'
import RowItem from './components/row-item/row-item.component'
import Footer from './components/footer/footer.component'
import BulletinBoard from './components/bulletin-board/bulletin-board.component'
import AddTodoContainer from './components/add-todo-container/add-todo-container.component'
import TodoItemsList from './components/todo-items-list/todo-item-list.component'

const App = () => {

const boardNotes = {
  title: 'Todo List App', 
  description: 'Welcome to the ClearPoint frontend technical test. We like to keep things simple, yet clean so your task(s) are as follows:',
  notes: [
    {
      id: 1, 
      message: 'Add the ability to add (POST) a Todo Item by calling the backend API'
    },
    {
      id: 2, 
      message: 'Display (GET) all the current Todo Items in the below grid and display them in any order you wish'
    },
    {
      id: 3, 
      message: "Bonus points for completing the 'Mark as completed' button code for allowing users to update and mark a specific Todo Item as completed and for displaying any relevant validation errors/ messages from the API in the UI"
    },
    {
      id: 4, 
      message: 'Feel free to add unit tests and refactor the component(s) as best you see fit'
    },
  ]}

  return (
    <div className="App">
      <Container>
        <RowItem>
          <Image data-testid="main-img" src="clearPointLogo.png" fluid rounded />
        </RowItem>
        <RowItem>
          <BulletinBoard body={boardNotes}/>
        </RowItem>
          <RowItem>
            <AddTodoContainer />
          </RowItem>
        <br />
        <RowItem>
          <TodoItemsList />
        </RowItem>
      </Container>
      <Footer copyright={"© 2021 Copyright:"} 
      description={"clearpoint.digital"} 
      href={"https://clearpoint.digital"} 
      target={"_blank"} 
      rel={"noreferrer"}/>
    </div>
  )
}

export default App
