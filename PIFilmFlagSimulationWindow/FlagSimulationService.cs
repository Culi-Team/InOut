using EQX.InOut.Virtual;
using PIFilmFlagSimulationWindow.FlagDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmFlagSimulationWindow
{
    public class FlagSimulationService
    {
        public FlagSimulationService()
        {
            InConveyorInput = VirtualDeviceRegistry.GetOrAddInputDevice<EInConveyorProcessInput>("InConveyorInput");

            InWorkConveyorInput = VirtualDeviceRegistry.GetOrAddInputDevice<EWorkConveyorProcessInput>("InWorkConveyorInput");
            InWorkConveyorOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EWorkConveyorProcessOutput>("InWorkConveyorOutput");

            BufferConveyorInput = VirtualDeviceRegistry.GetOrAddInputDevice<EBufferConveyorProcessInput>("BufferConveyorInput");
            BufferConveyorOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EBufferConveyorProcessOutput>("BufferConveyorOutput");

            OutWorkConveyorInput = VirtualDeviceRegistry.GetOrAddInputDevice<EWorkConveyorProcessInput>("OutWorkConveyorInput");
            OutWorkConveyorOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EWorkConveyorProcessOutput>("OutWorkConveyorOutput");

            OutConveyorInput = VirtualDeviceRegistry.GetOrAddInputDevice<EOutConveyorProcessInput>("OutConveyorInput");
            OutConveyorOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EOutConveyorProcessOutput>("OutConveyorOutput");

            RobotLoadInput = VirtualDeviceRegistry.GetOrAddInputDevice<ERobotLoadProcessInput>("RobotLoadInput");
            RobotLoadOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ERobotLoadProcessOutput>("RobotLoadOutput");

            VinylCleanInput = VirtualDeviceRegistry.GetOrAddInputDevice<EVinylCleanProcessInput>("VinylCleanInput");
            VinylCleanOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EVinylCleanProcessOutput>("VinylCleanOutput");

            FixtureAlignInput = VirtualDeviceRegistry.GetOrAddInputDevice<EFixtureAlignProcessInput>("FixtureAlignInput");
            FixtureAlignOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EFixtureAlignProcessOutput>("FixtureAlignOutput");

            RemoveFilmInput = VirtualDeviceRegistry.GetOrAddInputDevice<ERemoveFilmProcessInput>("RemoveFilmInput");
            RemoveFilmOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ERemoveFilmProcessOutput>("RemoveFilmOutput");

            TransferFixtureInput = VirtualDeviceRegistry.GetOrAddInputDevice<ETransferFixtureProcessInput>("TransferFixtureInput");
            TransferFixtureOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferFixtureProcessOutput>("TransferFixtureOutput");

            DetachInput = VirtualDeviceRegistry.GetOrAddInputDevice<EDetachProcessInput>("DetachInput");
            DetachOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EDetachProcessOutput>("DetachOutput");

            GlassTransferInput = VirtualDeviceRegistry.GetOrAddInputDevice<EGlassTransferProcessInput>("GlassTransferInput");
            GlassTransferOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EGlassTransferProcessOutput>("GlassTransferOutput");

            GlassAlignLeftInput = VirtualDeviceRegistry.GetOrAddInputDevice<EGlassAlignProcessInput>("GlassAlignLeftInput");
            GlassAlignLeftOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EGlassAlignProcessOutput>("GlassAlignLeftOutput");

            GlassAlignRightInput = VirtualDeviceRegistry.GetOrAddInputDevice<EGlassAlignProcessInput>("GlassAlignRightInput");
            GlassAlignRightOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EGlassAlignProcessOutput>("GlassAlignRightOutput");

            TransferInShuttleLeftInput = VirtualDeviceRegistry.GetOrAddInputDevice<ETransferInShuttleProcessInput>("TransferInShuttleLeftInput");
            TransferInShuttleLeftOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferInShuttleProcessOutput>("TransferInShuttleLeftOutput");

            TransferInShuttleRightInput = VirtualDeviceRegistry.GetOrAddInputDevice<ETransferInShuttleProcessInput>("TransferInShuttleRightInput");
            TransferInShuttleRightOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferInShuttleProcessOutput>("TransferInShuttleRightOutput");

            WetCleanLeftInput = VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("WETCleanLeftInput");
            WetCleanLeftOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("WETCleanLeftOutput");

            WetCleanRightInput = VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("WETCleanRightInput");
            WetCleanRightOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("WETCleanRightOutput");

            TransferRotationLeftInput = VirtualDeviceRegistry.GetOrAddInputDevice<ETransferRotationProcessInput>("TransferRotationLeftInput");
            TransferRotationLeftOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferRotationProcessOutput>("TransferRotationLeftOutput");

            TransferRotationRightInput = VirtualDeviceRegistry.GetOrAddInputDevice<ETransferRotationProcessInput>("TransferRotationRightInput");
            TransferRotationRightOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferRotationProcessOutput>("TransferRotationRightOutput");

            AfCleanLeftInput = VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("AFCleanLeftInput");
            AfCleanLeftOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("AFCleanLeftOutput");

            AfCleanRightInput = VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("AFCleanRightInput");
            AfCleanRightOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("AFCleanRightOutput");

            UnloadTransferLeftInput = VirtualDeviceRegistry.GetOrAddInputDevice<EUnloadTransferProcessInput>("UnloadTransferLeftInput");
            UnloadTransferLeftOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EUnloadTransferProcessOutput>("UnloadTransferLeftOutput");

            UnloadTransferRightInput = VirtualDeviceRegistry.GetOrAddInputDevice<EUnloadTransferProcessInput>("UnloadTransferRightInput");
            UnloadTransferRightOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EUnloadTransferProcessOutput>("UnloadTransferRightOutput");

            UnloadAlignInput = VirtualDeviceRegistry.GetOrAddInputDevice<EUnloadAlignProcessInput>("UnloadAlignInput");
            UnloadAlignOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<EUnloadAlignProcessOutput>("UnloadAlignOutput");

            RobotUnloadInput = VirtualDeviceRegistry.GetOrAddInputDevice<ERobotUnloadProcessInput>("RobotUnloadInput");
            RobotUnloadOutput = VirtualDeviceRegistry.GetOrAddOutputDevice<ERobotUnloadProcessOutput>("RobotUnloadOutput");

            InitializeDevices();
            SetupMappings();
        }

        private void InitializeDevices()
        {
            EnsureInitialized(InConveyorInput);

            EnsureInitialized(InWorkConveyorInput);
            EnsureInitialized(InWorkConveyorOutput);

            EnsureInitialized(BufferConveyorInput);
            EnsureInitialized(BufferConveyorOutput);

            EnsureInitialized(OutWorkConveyorInput);
            EnsureInitialized(OutWorkConveyorOutput);

            EnsureInitialized(OutConveyorInput);
            EnsureInitialized(OutConveyorOutput);

            EnsureInitialized(RobotLoadInput);
            EnsureInitialized(RobotLoadOutput);

            EnsureInitialized(VinylCleanInput);
            EnsureInitialized(VinylCleanOutput);

            EnsureInitialized(FixtureAlignInput);
            EnsureInitialized(FixtureAlignOutput);

            EnsureInitialized(RemoveFilmInput);
            EnsureInitialized(RemoveFilmOutput);

            EnsureInitialized(TransferFixtureInput);
            EnsureInitialized(TransferFixtureOutput);

            EnsureInitialized(DetachInput);
            EnsureInitialized(DetachOutput);

            EnsureInitialized(GlassTransferInput);
            EnsureInitialized(GlassTransferOutput);

            EnsureInitialized(GlassAlignLeftInput);
            EnsureInitialized(GlassAlignLeftOutput);

            EnsureInitialized(GlassAlignRightInput);
            EnsureInitialized(GlassAlignRightOutput);

            EnsureInitialized(TransferInShuttleLeftInput);
            EnsureInitialized(TransferInShuttleLeftOutput);

            EnsureInitialized(TransferInShuttleRightInput);
            EnsureInitialized(TransferInShuttleRightOutput);

            EnsureInitialized(WetCleanLeftInput);
            EnsureInitialized(WetCleanLeftOutput);

            EnsureInitialized(WetCleanRightInput);
            EnsureInitialized(WetCleanRightOutput);

            EnsureInitialized(TransferRotationLeftInput);
            EnsureInitialized(TransferRotationLeftOutput);

            EnsureInitialized(TransferRotationRightInput);
            EnsureInitialized(TransferRotationRightOutput);

            EnsureInitialized(AfCleanLeftInput);
            EnsureInitialized(AfCleanLeftOutput);

            EnsureInitialized(AfCleanRightInput);
            EnsureInitialized(AfCleanRightOutput);

            EnsureInitialized(UnloadTransferLeftInput);
            EnsureInitialized(UnloadTransferLeftOutput);

            EnsureInitialized(UnloadTransferRightInput);
            EnsureInitialized(UnloadTransferRightOutput);

            EnsureInitialized(UnloadAlignInput);
            EnsureInitialized(UnloadAlignOutput);

            EnsureInitialized(RobotUnloadInput);
            EnsureInitialized(RobotUnloadOutput);
        }

        private static void EnsureInitialized<TEnum>(VirtualInputDevice<TEnum> device) where TEnum : Enum
        {
            if (device.Inputs.Count == 0)
            {
                device.Initialize();
            }
        }

        private static void EnsureInitialized<TEnum>(VirtualOutputDevice<TEnum> device) where TEnum : Enum
        {
            if (device.Outputs.Count == 0)
            {
                device.Initialize();
            }
        }

        private void SetupMappings()
        {
            //InConveyor Input Mapping
            InConveyorInput.Mapping((int)EInConveyorProcessInput.REQUEST_CST_IN,
                InWorkConveyorOutput, (int)EWorkConveyorProcessOutput.REQUEST_CST_IN);

            //InWorkConveyor Input Mapping
            InWorkConveyorInput.Mapping((int)EWorkConveyorProcessInput.ROBOT_PICK_PLACE_CST_DONE,
                RobotLoadOutput, (int)ERobotLoadProcessOutput.ROBOT_PICK_IN_CST_DONE);
            InWorkConveyorInput.Mapping((int)EWorkConveyorProcessInput.NEXT_CONVEYOR_READY,
                BufferConveyorOutput, (int)EBufferConveyorProcessOutput.BUFFER_CONVEYOR_READY);

            //BufferConveyor Input Mapping
            BufferConveyorInput.Mapping((int)EBufferConveyorProcessInput.IN_WORK_CONVEYOR_REQUEST_CST_OUT,
                InWorkConveyorOutput, (int)EWorkConveyorProcessOutput.REQUEST_CST_OUT);
            BufferConveyorInput.Mapping((int)EBufferConveyorProcessInput.OUT_WORK_CONVEYOR_REQUEST_CST_IN,
                OutWorkConveyorOutput, (int)EWorkConveyorProcessOutput.REQUEST_CST_IN);

            //OutWorkConveyor Input Mapping
            OutWorkConveyorInput.Mapping((int)EWorkConveyorProcessInput.ROBOT_PICK_PLACE_CST_DONE,
                RobotLoadOutput, (int)ERobotLoadProcessOutput.ROBOT_PLACE_OUT_CST_DONE);
            OutWorkConveyorInput.Mapping((int)EWorkConveyorProcessInput.NEXT_CONVEYOR_READY,
                OutConveyorOutput, (int)EOutConveyorProcessOutput.OUT_CONVEYOR_READY);

            //OutConveyor Input Mapping
            OutConveyorInput.Mapping((int)EOutConveyorProcessInput.OUT_WORK_CONVEYOR_REQUEST_CST_OUT,
                OutWorkConveyorOutput, (int)EWorkConveyorProcessOutput.REQUEST_CST_OUT);

            //Robot Load Input Mapping
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.IN_CST_READY,
                InWorkConveyorOutput, (int)EWorkConveyorProcessOutput.CST_READY);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.OUT_CST_READY,
                OutWorkConveyorOutput, (int)EWorkConveyorProcessOutput.CST_READY);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_LOAD,
                VinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_LOAD);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_LOAD_DONE,
                VinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_LOAD_DONE);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_UNLOAD,
                VinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_UNLOAD);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE,
                VinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.FIXTURE_ALIGN_REQ_LOAD,
                FixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_REQ_LOAD);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.REMOVE_FILM_REQ_UNLOAD,
                RemoveFilmOutput, (int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.IN_CST_PICK_DONE_RECEIVED,
                InWorkConveyorOutput, (int)EWorkConveyorProcessOutput.IN_CST_PICK_PLACE_DONE_RECEIVED);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.OUT_CST_PLACE_DONE_RECEIVED,
                OutWorkConveyorOutput, (int)EWorkConveyorProcessOutput.IN_CST_PICK_PLACE_DONE_RECEIVED);
            RobotLoadInput.Mapping((int)ERobotLoadProcessInput.FIXTURE_ALIGN_LOAD_DONE_RECEIVED,
                FixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_LOAD_DONE_RECEIVED);

            //Vinyl Clean Input Mapping
            VinylCleanInput.Mapping((int)EVinylCleanProcessInput.VINYL_CLEAN_LOAD_DONE,
                RobotLoadOutput, (int)ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE);
            VinylCleanInput.Mapping((int)EVinylCleanProcessInput.VINYL_CLEAN_UNLOAD_DONE,
                RobotLoadOutput, (int)ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE);

            //Fixture Align Input Mapping
            FixtureAlignInput.Mapping((int)EFixtureAlignProcessInput.FIXTURE_ALIGN_LOAD_DONE,
                RobotLoadOutput, (int)ERobotLoadProcessOutput.FIXTURE_ALIGN_LOAD_DONE);
            FixtureAlignInput.Mapping((int)EFixtureAlignProcessInput.FIXTURE_TRANSFER_DONE,
                TransferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);

            //Remove Film Input Mapping
            RemoveFilmInput.Mapping((int)ERemoveFilmProcessInput.REMOVE_FILM_UNLOAD_DONE,
                RobotLoadOutput, (int)ERobotLoadProcessOutput.REMOVE_FILM_UNLOAD_DONE);
            RemoveFilmInput.Mapping((int)ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE,
                TransferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);

            //Transfer Fixture Input Mapping
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.DETACH_ORIGIN_DONE,
                DetachOutput, (int)EDetachProcessOutput.DETACH_ORIGIN_DONE);
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.DETACH_DONE,
                DetachOutput, (int)EDetachProcessOutput.DETACH_DONE);
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.FIXTURE_ALIGN_DONE,
                FixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_DONE);
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.REMOVE_FILM_DONE,
                RemoveFilmOutput, (int)ERemoveFilmProcessOutput.REMOVE_FILM_DONE);
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.ALIGN_TRANSFER_FIXTURE_DONE_RECEIVED,
                FixtureAlignOutput, (int)EFixtureAlignProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.DETACH_TRANSFER_FIXTURE_DONE_RECEIVED,
                DetachOutput, (int)EDetachProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);
            TransferFixtureInput.Mapping((int)ETransferFixtureProcessInput.REMOVE_FILM_TRANSFER_FIXTURE_DONE_RECEIVED,
                RemoveFilmOutput, (int)ERemoveFilmProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);

            //Detach Input Mapping
            DetachInput.Mapping((int)EDetachProcessInput.FIXTURE_TRANSFER_DONE,
                TransferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);
            DetachInput.Mapping((int)EDetachProcessInput.GLASS_TRANSFER_PICK_DONE,
                GlassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_PICK_DONE);

            //Glass Transfer Input Mapping
            GlassTransferInput.Mapping((int)EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS,
                DetachOutput, (int)EDetachProcessOutput.DETACH_REQ_UNLOAD_GLASS);
            GlassTransferInput.Mapping((int)EGlassTransferProcessInput.GLASS_TRANSFER_PICK_DONE_RECEIVED,
                DetachOutput, (int)EDetachProcessOutput.GLASS_TRANSFER_PICK_DONE_RECEIVED);
            GlassTransferInput.Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_REQ_GLASS,
                GlassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS);
            GlassTransferInput.Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_REQ_GLASS,
                GlassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS);
            GlassTransferInput.Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_PLACE_DONE_RECEIVED,
                GlassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED);
            GlassTransferInput.Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_PLACE_DONE_RECEIVED,
                GlassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED);

            //Glass Align Input Mapping - Left
            GlassAlignLeftInput.Mapping((int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE,
                GlassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_LEFT_PLACE_DONE);
            GlassAlignLeftInput.Mapping((int)EGlassAlignProcessInput.TRANSFER_IN_SHUTTLE_PICK_DONE,
                TransferInShuttleLeftOutput, (int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE);

            //Glass Align Input Mapping - Right
            GlassAlignRightInput.Mapping((int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE,
                GlassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_RIGHT_PLACE_DONE);
            GlassAlignRightInput.Mapping((int)EGlassAlignProcessInput.TRANSFER_IN_SHUTTLE_PICK_DONE,
                TransferInShuttleRightOutput, (int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE);

            //TransferInShuttle Input Mapping - Left
            TransferInShuttleLeftInput.Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK,
                GlassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK);
            TransferInShuttleLeftInput.Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED,
                GlassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED);
            TransferInShuttleLeftInput.Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD,
                WetCleanLeftOutput, (int)ECleanProcessOutput.REQ_LOAD);
            TransferInShuttleLeftInput.Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED,
                WetCleanLeftOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);

            //TransferInShuttle Input Mapping - Right
            TransferInShuttleRightInput.Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK,
                GlassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK);
            TransferInShuttleRightInput.Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED,
                GlassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED);
            TransferInShuttleRightInput.Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD,
                WetCleanRightOutput, (int)ECleanProcessOutput.REQ_LOAD);
            TransferInShuttleRightInput.Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED,
                WetCleanRightOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);

            //WET Clean Input Mapping - Left
            WetCleanLeftInput.Mapping((int)ECleanProcessInput.LOAD_DONE,
                TransferInShuttleLeftOutput, (int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE);
            WetCleanLeftInput.Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                TransferRotationLeftOutput, (int)ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE);
            WetCleanLeftInput.Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                TransferRotationLeftOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK);

            //WET Clean Input Mapping - Right
            WetCleanRightInput.Mapping((int)ECleanProcessInput.LOAD_DONE,
                TransferInShuttleRightOutput, (int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE);
            WetCleanRightInput.Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                TransferRotationRightOutput, (int)ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE);
            WetCleanRightInput.Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                TransferRotationRightOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK);

            //AF Clean Input Mapping - Left
            AfCleanLeftInput.Mapping((int)ECleanProcessInput.LOAD_DONE,
                TransferRotationLeftOutput, (int)ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE);
            AfCleanLeftInput.Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                UnloadTransferLeftOutput, (int)EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE);
            AfCleanLeftInput.Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                TransferRotationLeftOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE);

            //AF Clean Input Mapping - Right
            AfCleanRightInput.Mapping((int)ECleanProcessInput.LOAD_DONE,
                TransferRotationRightOutput, (int)ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE);
            AfCleanRightInput.Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                UnloadTransferRightOutput, (int)EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE);
            AfCleanRightInput.Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                TransferRotationRightOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE);

            //Transfer Rotation Input Mapping - Left
            TransferRotationLeftInput.Mapping((int)ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD,
                WetCleanLeftOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            TransferRotationLeftInput.Mapping((int)ETransferRotationProcessInput.WET_CLEAN_UNLOAD_DONE_RECEIVED,
                WetCleanLeftOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            TransferRotationLeftInput.Mapping((int)ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD,
                AfCleanLeftOutput, (int)ECleanProcessOutput.REQ_LOAD);
            TransferRotationLeftInput.Mapping((int)ETransferRotationProcessInput.AF_CLEAN_LOAD_DONE_RECEIVED,
                AfCleanLeftOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);

            //Transfer Rotation Input Mapping - Right
            TransferRotationRightInput.Mapping((int)ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD,
                WetCleanRightOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            TransferRotationRightInput.Mapping((int)ETransferRotationProcessInput.WET_CLEAN_UNLOAD_DONE_RECEIVED,
                WetCleanRightOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            TransferRotationRightInput.Mapping((int)ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD,
                AfCleanRightOutput, (int)ECleanProcessOutput.REQ_LOAD);
            TransferRotationRightInput.Mapping((int)ETransferRotationProcessInput.AF_CLEAN_LOAD_DONE_RECEIVED,
                AfCleanRightOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);

            //Unload Transfer Input Mapping - Left
            UnloadTransferLeftInput.Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD,
                AfCleanLeftOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            UnloadTransferLeftInput.Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_UNLOAD_DONE_RECEIVED,
                AfCleanLeftOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            UnloadTransferLeftInput.Mapping((int)EUnloadTransferProcessInput.UNLOAD_TRANSFER_UNLOADING,
                UnloadTransferRightOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_UNLOADING);
            UnloadTransferLeftInput.Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_READY,
                UnloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY);
            UnloadTransferLeftInput.Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED,
                UnloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED);

            //Unload Transfer Input Mapping - Right
            UnloadTransferRightInput.Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD,
                AfCleanRightOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            UnloadTransferRightInput.Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_UNLOAD_DONE_RECEIVED,
                AfCleanRightOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            UnloadTransferRightInput.Mapping((int)EUnloadTransferProcessInput.UNLOAD_TRANSFER_UNLOADING,
                UnloadTransferLeftOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_UNLOADING);
            UnloadTransferRightInput.Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_READY,
                UnloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY);
            UnloadTransferRightInput.Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED,
                UnloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED);

            //Unload Align Input Mapping
            UnloadAlignInput.Mapping((int)EUnloadAlignProcessInput.UNLOAD_TRANSFER_LEFT_PLACE_DONE,
                UnloadTransferLeftOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_PLACE_DONE);
            UnloadAlignInput.Mapping((int)EUnloadAlignProcessInput.UNLOAD_TRANSFER_RIGHT_PLACE_DONE,
                UnloadTransferRightOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_PLACE_DONE);
            UnloadAlignInput.Mapping((int)EUnloadAlignProcessInput.ROBOT_UNLOAD_PICK_DONE,
                RobotUnloadOutput, (int)ERobotUnloadProcessOutput.ROBOT_UNLOAD_PICK_DONE);

            RobotUnloadInput.Mapping((int)ERobotUnloadProcessInput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD,
                UnloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD);
            RobotUnloadInput.Mapping((int)ERobotUnloadProcessInput.UNLOAD_ALIGN_UNLOAD_DONE_RECEIVED,
                RobotUnloadOutput, (int)ERobotUnloadProcessInput.UNLOAD_ALIGN_UNLOAD_DONE_RECEIVED);
        }

        #region Properties
        public VirtualInputDevice<EInConveyorProcessInput> InConveyorInput { get; }

        public VirtualInputDevice<EWorkConveyorProcessInput> InWorkConveyorInput { get; }
        public VirtualOutputDevice<EWorkConveyorProcessOutput> InWorkConveyorOutput { get; }

        public VirtualInputDevice<EBufferConveyorProcessInput> BufferConveyorInput { get; }
        public VirtualOutputDevice<EBufferConveyorProcessOutput> BufferConveyorOutput { get; }

        public VirtualInputDevice<EWorkConveyorProcessInput> OutWorkConveyorInput { get; }
        public VirtualOutputDevice<EWorkConveyorProcessOutput> OutWorkConveyorOutput { get; }

        public VirtualInputDevice<EOutConveyorProcessInput> OutConveyorInput { get; }
        public VirtualOutputDevice<EOutConveyorProcessOutput> OutConveyorOutput { get; }

        public VirtualInputDevice<ERobotLoadProcessInput> RobotLoadInput { get; }
        public VirtualOutputDevice<ERobotLoadProcessOutput> RobotLoadOutput { get; }

        public VirtualInputDevice<EVinylCleanProcessInput> VinylCleanInput { get; }
        public VirtualOutputDevice<EVinylCleanProcessOutput> VinylCleanOutput { get; }

        public VirtualInputDevice<EFixtureAlignProcessInput> FixtureAlignInput { get; }
        public VirtualOutputDevice<EFixtureAlignProcessOutput> FixtureAlignOutput { get; }

        public VirtualInputDevice<ERemoveFilmProcessInput> RemoveFilmInput { get; }
        public VirtualOutputDevice<ERemoveFilmProcessOutput> RemoveFilmOutput { get; }

        public VirtualInputDevice<ETransferFixtureProcessInput> TransferFixtureInput { get; }
        public VirtualOutputDevice<ETransferFixtureProcessOutput> TransferFixtureOutput { get; }

        public VirtualInputDevice<EDetachProcessInput> DetachInput { get; }
        public VirtualOutputDevice<EDetachProcessOutput> DetachOutput { get; }

        public VirtualInputDevice<EGlassTransferProcessInput> GlassTransferInput { get; }
        public VirtualOutputDevice<EGlassTransferProcessOutput> GlassTransferOutput { get; }

        public VirtualInputDevice<EGlassAlignProcessInput> GlassAlignLeftInput { get; }
        public VirtualOutputDevice<EGlassAlignProcessOutput> GlassAlignLeftOutput { get; }

        public VirtualInputDevice<EGlassAlignProcessInput> GlassAlignRightInput { get; }
        public VirtualOutputDevice<EGlassAlignProcessOutput> GlassAlignRightOutput { get; }

        public VirtualInputDevice<ETransferInShuttleProcessInput> TransferInShuttleLeftInput { get; }
        public VirtualOutputDevice<ETransferInShuttleProcessOutput> TransferInShuttleLeftOutput { get; }

        public VirtualInputDevice<ETransferInShuttleProcessInput> TransferInShuttleRightInput { get; }
        public VirtualOutputDevice<ETransferInShuttleProcessOutput> TransferInShuttleRightOutput { get; }

        public VirtualInputDevice<ECleanProcessInput> WetCleanLeftInput { get; }
        public VirtualOutputDevice<ECleanProcessOutput> WetCleanLeftOutput { get; }

        public VirtualInputDevice<ECleanProcessInput> WetCleanRightInput { get; }
        public VirtualOutputDevice<ECleanProcessOutput> WetCleanRightOutput { get; }

        public VirtualInputDevice<ETransferRotationProcessInput> TransferRotationLeftInput { get; }
        public VirtualOutputDevice<ETransferRotationProcessOutput> TransferRotationLeftOutput { get; }

        public VirtualInputDevice<ETransferRotationProcessInput> TransferRotationRightInput { get; }
        public VirtualOutputDevice<ETransferRotationProcessOutput> TransferRotationRightOutput { get; }

        public VirtualInputDevice<ECleanProcessInput> AfCleanLeftInput { get; }
        public VirtualOutputDevice<ECleanProcessOutput> AfCleanLeftOutput { get; }

        public VirtualInputDevice<ECleanProcessInput> AfCleanRightInput { get; }
        public VirtualOutputDevice<ECleanProcessOutput> AfCleanRightOutput { get; }

        public VirtualInputDevice<EUnloadTransferProcessInput> UnloadTransferLeftInput { get; }
        public VirtualOutputDevice<EUnloadTransferProcessOutput> UnloadTransferLeftOutput { get; }

        public VirtualInputDevice<EUnloadTransferProcessInput> UnloadTransferRightInput { get; }
        public VirtualOutputDevice<EUnloadTransferProcessOutput> UnloadTransferRightOutput { get; }

        public VirtualInputDevice<EUnloadAlignProcessInput> UnloadAlignInput { get; }
        public VirtualOutputDevice<EUnloadAlignProcessOutput> UnloadAlignOutput { get; }

        public VirtualInputDevice<ERobotUnloadProcessInput> RobotUnloadInput { get; }
        public VirtualOutputDevice<ERobotUnloadProcessOutput> RobotUnloadOutput { get; }
        #endregion
    }
}

