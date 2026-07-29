import { Code, Info } from 'lucide-react';

export default function ConceptBadge({ concept, title, description, codeSnippet }) {
  return (
    <div className="concept-header">
      <div className="concept-tag">
        <Info size={14} /> {concept}
      </div>
      <h1 className="concept-title">{title}</h1>
      <p className="concept-desc">{description}</p>
      {codeSnippet && (
        <div className="code-snippet">
          <Code size={14} style={{ display: 'inline', marginRight: '6px' }} />
          <code>{codeSnippet}</code>
        </div>
      )}
    </div>
  );
}
