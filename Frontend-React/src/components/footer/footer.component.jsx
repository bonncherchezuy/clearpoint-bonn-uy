
const Footer = ({copyright, description, href, target, rel,}) => (
  <footer data-testid="footer" className="page-footer font-small teal pt-4">
    <div className="footer-copyright text-center py-3">
      {copyright}
      <a href={href} target={target} rel={rel}>
        {description}
      </a>
    </div>
  </footer>
);

export default Footer