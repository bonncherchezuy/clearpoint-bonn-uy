import { render, screen } from '@testing-library/react'
import App from './App'

describe('App Tests', () => {
  describe('When rendering App',() =>{

    beforeEach(() => {
      render(<App />)
    });

    it('should have main image', () => {

      const actual = screen.getByTestId("main-img");
      expect(actual).toBeTruthy();

    });

    it('should have the bulletin board component', () => {
      const actual = screen.getByTestId("bulletin-board");
      
      expect(actual).toBeTruthy();
    });

    it('should have add to do container', () => {

      const actual = screen.getByTestId("add-container");
      expect(actual).toBeTruthy();

    });

    it('should have the footer element', () => {

      const actual = screen.getByTestId("footer");
      expect(actual).toBeTruthy();
      expect(actual.tagName).toEqual('FOOTER');
    });
  });
});