using System.Collections;
using UnityEngine;

namespace LivelyChatBubbles
{
	public class ChatMouthpiece : MonoBehaviour
	{
		[Tooltip("Canvas where all chat bubble instances will be created.")]
		public Canvas Canvas;

		[Tooltip("Name displayed when spoken.")]
		public string ChatName;

		[Tooltip("Prefab that is instanciated when spoken.")]
		public ChatBubble ChatBubblePrefab;

		[Tooltip("Chat output information used when spoken such as characters per second, etc.")]
		public ChatOutputProfile ChatOutputProfile;

		[Tooltip("Chat anchor to attach all instanciated bubbles.")]
		public ChatAnchor ChatAnchor;

		[Tooltip("Audio information to play while speaking.")]
		public AudioSource AudioSource;

		public bool isSpeaking { get; private set; }

		ChatBubble chatBubble;
		ChatOutputProcesser chatProcesser;

		void Start()
		{
			if (ChatAnchor && ChatAnchor.AttachedBubble)
				GameObject.Destroy(ChatAnchor.AttachedBubble.gameObject);
		}

		public void Speak(string message)
		{
			// Canvas = null;
			// ChatAnchor = null;
			// ChatOutputProfile = null;
			// ChatBubblePrefab = null;
			// chatBubble = null;

			// do not speak if already speaking
			if (Canvas && ChatAnchor && ChatOutputProfile && ChatBubblePrefab && chatBubble == null)
			{
				UnityEngine.Debug.Log("Here 1");

				isSpeaking = true;
				chatBubble = GameObject.Instantiate(ChatBubblePrefab, new Vector3(-100000, -10000, 0), Quaternion.identity, Canvas.transform);
				ChatAnchor.BindAttachedBubble(chatBubble);
				chatBubble.BindNameValue(ChatName);
				chatProcesser = chatBubble.gameObject.AddComponent<ChatOutputProcesser>();
				chatProcesser.enabled = false;
				chatProcesser.Profile = ChatOutputProfile;
				chatProcesser.Value = message;
				chatProcesser.AudioSource = AudioSource;
				chatProcesser.enabled = true;
				chatBubble.gameObject.SetActive(true);
				StartCoroutine(WaitForFinished());

				UnityEngine.Debug.Log("Here 2");
			}
		}

		IEnumerator WaitForFinished()
		{
			UnityEngine.Debug.Log("Here 3");

			yield return new WaitForSeconds(1);
			UnityEngine.Debug.Log("Inside A");
			while (chatProcesser.enabled)
				yield return new WaitForSeconds(0.1f);
			UnityEngine.Debug.Log("Inside B");
			GameObject.Destroy(chatBubble.gameObject);
			UnityEngine.Debug.Log("Inside C");
			chatBubble = null;
			chatProcesser = null;
			isSpeaking = false;

			UnityEngine.Debug.Log("Here 4");
		}
	}
}

