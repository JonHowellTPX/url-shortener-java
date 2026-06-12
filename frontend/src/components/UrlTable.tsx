import type { UrlListItem } from '../types/api';

interface UrlTableProps {
  urls: UrlListItem[];
  onDelete: (alias: string) => void;
}

export function UrlTable({ urls, onDelete }: UrlTableProps) {
  if (urls.length === 0) {
    return (
      <div className="empty-state">
        <p>No shortened URLs yet. Paste a long URL above to get started.</p>
      </div>
    );
  }

  return (
    <div className="url-table-wrapper">
      <table className="url-table" aria-label="Shortened URLs">
        <thead>
          <tr>
            <th scope="col">Short URL</th>
            <th scope="col">Destination</th>
            <th scope="col">
              <span className="sr-only">Actions</span>
            </th>
          </tr>
        </thead>
        <tbody>
          {urls.map((item) => (
            <tr key={item.alias}>
              <td>
                <a
                  href={item.shortUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="short-url-link"
                >
                  {item.shortUrl}
                </a>
              </td>
              <td>
                <span className="full-url" title={item.fullUrl}>
                  {item.fullUrl}
                </span>
              </td>
              <td className="actions-cell">
                <button
                  className="btn-danger"
                  onClick={() => onDelete(item.alias)}
                  aria-label={`Delete ${item.shortUrl}`}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
