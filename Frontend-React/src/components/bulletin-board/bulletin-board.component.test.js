import { render, screen } from '@testing-library/react'
import BulletinBoard from './bulletin-board.component'

describe('BulletinBoard Test', () => {
  describe('When rendering BulletinBoard', () => {
    
    let boardNotes;

    beforeEach(() => {
      boardNotes = {
        title: 'Test Title', 
        description: 'Test Description',
        notes: [
          { id: 1, message: 'Test note 1'},
          { id: 2, message: 'Test note 2'},
          { id: 3, message: 'Test note 3'},
          { id: 4, message: 'Test note 4'},
          { id: 5, message: 'Test note 5'},
        ]}
      render(<BulletinBoard body={boardNotes}/>);
    });

    it('should render the title text', () => {
      const actual = screen.getByText("Test Title");
      expect(actual).toBeInTheDocument()
    });

    it('should render the description text', () => {
      const actual = screen.getByText("Test Description");
      expect(actual).toBeInTheDocument()
    });

    it('should render all the notes', () => {

      const actual = screen.getAllByRole('listitem');

      expect(actual).toHaveLength(boardNotes.notes.length);

      actual.forEach((item, index) => {
        expect(item).toHaveTextContent(boardNotes.notes[index].message);
      });

    });

  });

});