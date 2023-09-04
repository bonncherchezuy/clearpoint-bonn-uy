import { render, screen } from '@testing-library/react'
import Footer from './footer.component'

describe('Footer Tests', () => {
  let footerComponent;

  beforeEach(()=>{
    footerComponent = <Footer copyright={"© test Copyright:"} description={"test description"} href={"https://testlink"} target={"_blank"} rel={"noreferrer"} />;
  });

  it('should be a footer html element', () => {
    render(footerComponent)
    const actual = screen.getByTestId("footer");
    
    expect(actual).toBeTruthy();
    expect(actual.tagName).toEqual('FOOTER');
  });

  it('should render the copyright text', () => {
    render(footerComponent);
    const copyrightProp = screen.getByText("© test Copyright:")

    expect(copyrightProp).toBeInTheDocument()
  });

  describe('when rendering the link', () => {
    it('should have the given description as the displayed text', () => {
      render(footerComponent)
      const textLink = screen.getByText("test description")
      
      expect(textLink).toBeTruthy();
      expect(textLink.tagName).toEqual('A');
    });

    it('should have the given href as the anchor tag href text', () => {
      render(footerComponent)
      const textLink = screen.getByText("test description")
      
      expect(textLink).toHaveAttribute('href', 'https://testlink');
    });
  });
});
