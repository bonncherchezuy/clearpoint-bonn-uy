import { render, screen } from '@testing-library/react'
import RowItem from './row-item.component'

describe('RowItem Test', () => {

  describe('When rendering RowItem', () => {

    it('should render the given children', () => {
      render(<RowItem>
        <div data-testid="testDiv">children successfully rendered</div>    
      </RowItem>)

      const childrenProp = screen.getByTestId("testDiv");
      expect(childrenProp).toBeInTheDocument()
    });

  });

});