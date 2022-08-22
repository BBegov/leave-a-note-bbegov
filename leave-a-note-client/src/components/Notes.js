import useAxiosFetch from '../hooks/useAxiosFetch';

const Notes = ({ url }) => {
    const { data: notes, isLoading, fetchError } = useAxiosFetch(url);

    return (
        <main className='notes'>
            {isLoading && <p className="statusMsg">Loading notes...</p>}
            {!isLoading && fetchError && <p className="statusMsg" style={{ color: "red" }}>{fetchError}</p>}
            {!isLoading && !fetchError && (notes.length ? (
                <>
                    {notes.map((note, index) => {
                        const date = new Date(note.publishDate);
                        const dateTime = `${date.toLocaleDateString('hu-HU')} ${date.toLocaleTimeString('hu-HU')}`;
                        return <p key={index}>{dateTime} - {note.noteText}</p>;
                    })
                    }
                </>
            ) : <p className="statusMsg">No notes to display.</p>)}
        </main >
    );
};

export default Notes;