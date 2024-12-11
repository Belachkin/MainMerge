using UnityEngine;
using NaughtyAttributes;
using System.Linq;
using System;

namespace Snippets.Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, ReorderableList] TutorialNode[] nodes;
        [SerializeField, ReadOnly] TutorialNode node;
        [SerializeField, ReadOnly] int stepIndex;

        [Header("Debug")]
        [SerializeField] TutorialNode debugNode;
        [SerializeReference, SubclassSelector] TutorialStep debugStep;

        public event Action<TutorialStep> OnStepBegin;
        public event Action<TutorialStep> OnStepComplete;

        TutorialDataController dataController;
        bool shouldUpdate;

        [Button]
        public void Begin()
        {
            dataController = new TutorialDataController();
            VerifyNodes();

            if (!IsTutorialCompleted())
            {
                SelectNode();
                BeginStep();
            }
        }

        public void Update()
        {
            if (shouldUpdate && node != null)
                node.Steps[stepIndex].OnUpdate();
        }

        void BeginStep()
        {
            if (IsTutorialCompleted()) return;

            node.Steps[stepIndex].CompleteEvent += CompleteStepAndContinue;
            node.Steps[stepIndex].Begin(this);

            shouldUpdate = true;
            OnStepBegin?.Invoke(node.Steps[stepIndex]);
        }

        void CompleteStepAndContinue()
        {
            CompleteStep();
            BeginStep();
        }

        void CompleteStep()
        {
            ExitStep();
            
            OnStepComplete?.Invoke(node.Steps[stepIndex]);
            stepIndex++;

            if (stepIndex >= node.Steps.Length)
            {
                dataController.CompleteNode(node.Key);
                stepIndex = 0;
                SelectNode();
            }
        }

        void ExitStep()
        {
            node.Steps[stepIndex].CompleteEvent -= CompleteStepAndContinue;
            shouldUpdate = false;
        }

        void SelectNode()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (!dataController.HasKey(nodes[i].Key))
                {
                    node = nodes[i];
                    return;
                }
            }
        }

        bool IsTutorialCompleted()
        {
            bool isCompleted = nodes.Where(x => !dataController.HasKey(x.Key)).Count() == 0;
            if (isCompleted) Debug.Log("Tutorial passed");
            return isCompleted;
        }

        /// <summary>
        /// Exception will be thrown if nodes with the same key present in collection
        /// </summary>
        void VerifyNodes()
        {
#if DEBUG
            nodes.ToDictionary(x => x.Key, x => x);
#endif
        }

        [Button]
        void TestStep()
        {
#if DEBUG
            ExitStep();
            node = debugNode;
            var targetStep = debugNode.Steps.First(x => x.GetType() == debugStep.GetType());
            stepIndex = debugNode.Steps.ToList().IndexOf(targetStep);
            BeginStep();
#endif
        }

        /// <summary>
        /// Debug purposes only
        /// </summary>
        public void TestStep(TutorialNode debugNode, TutorialStep debugStep)
        {
            this.debugNode = debugNode;
            this.debugStep = debugStep;
            TestStep();
        }
    }
}
